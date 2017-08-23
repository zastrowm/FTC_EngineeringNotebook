using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using EngineeringNotebook.Model;
using EngineeringNotebook.Mvvm;
using NotebookApp.Properties;
using Ookii.Dialogs.Wpf;

namespace NotebookApp.Tools
{
  /// <summary>
  /// Interaction logic for BatchProcessFiles.xaml
  /// </summary>
  public partial class BatchProcessFiles : Window
  {
    public BatchProcessFiles()
    {
      InitializeComponent();

      InputDirectory.Text = Settings.Default.LastInputDirectory;
      OutputDirectory.Text = Settings.Default.LastOutputDirectory;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
      base.OnClosing(e);

      Settings.Default.LastInputDirectory = InputDirectory.Text;
      Settings.Default.LastOutputDirectory = OutputDirectory.Text;
      Settings.Default.Save();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
      var button = (Button)sender;
      var target = (TextBox)button.Tag;

      var folderDialog = new VistaFolderBrowserDialog();
      if (true != folderDialog.ShowDialog())
        return;

      target.Text = folderDialog.SelectedPath;
    }

    private void ProcessDirectory(object sender, RoutedEventArgs e)
    {
      try
      {
        ProcessDirectory();
      }
      catch (Exception exception)
      {
        MessageBox.Show(exception.ToString(), "Error");
      }
    }

    private void ProcessDirectory()
    {
      var inputDirectory = InputDirectory.Text;
      var outputDirectory = OutputDirectory.Text;

      if (!CheckValidness(inputDirectory, outputDirectory))
      {
        MessageBox.Show("Invalid Input or Output Directory");
        return;
      }

      var pathToCsv = Path.Combine(inputDirectory, "mapping.csv");
      if (!File.Exists(pathToCsv))
      {
        MessageBox.Show("mapping.csv does not exist in Input Directory");
        return;
      }

      var mapping = LoadMapping(inputDirectory);

      var pages = Directory.GetFiles(inputDirectory, "*.engpage")
                           .Select(filename =>
                                   {
                                     var model = Serializer.Deserialize(filename);
                                     var vm = new PageEntryViewModel().Load(model);
                                     var dateString = model.EntryDate?.ToString("yy-MM-dd", null);

                                     string practiceName = null;
                                     if (dateString == null || !mapping.TryGetValue(dateString, out practiceName))
                                     {
                                       MessageBox.Show($"No mapping for {dateString}");
                                     }

                                     return new
                                            {
                                              vm,
                                              filename,
                                              practiceName
                                            };
                                   })
                           .Where(p => p.practiceName != null)
                           .ToList();

      foreach (var page in pages)
      {
        var outputFilename = Path.Combine(outputDirectory, Path.GetFileNameWithoutExtension(page.filename) + ".xps");
        var didPrintSuccessfully = PrintPageToFile(page.vm,
                                                   page.practiceName,
                                                   outputFilename);
        if (!didPrintSuccessfully)
        {
          break;
        }
      }
    }

    private static Dictionary<string, string> LoadMapping(string directory)
    {
      return File.ReadLines(Path.Combine(directory, "mapping.csv"))
                 .Select(line =>
                         {
                           var parts = line.Split(new[] { ',' }, 2);
                           var date = parts[0];
                           var name = parts[1];

                           return new
                                  {
                                    date,
                                    name
                                  };
                         }).ToDictionary(it => it.date, it => it.name);
    }

    private bool PrintPageToFile(PageEntryViewModel viewmodel, string practiceName, string filename)
    {
      try
      {
        new PageCreator(viewmodel, practiceName).SaveToFile(filename);
      }
      catch (Exception e)
      {
        MessageBox.Show(e.ToString(), $"Error while saving '{filename}'");
        return false;
      }

      return true;
    }

    private bool CheckValidness(string inputDirectory, string outputDirectory)
    {
      try
      {
        return Directory.Exists(inputDirectory)
               && Directory.Exists(outputDirectory);
      }
      catch
      {
        return false;
      }
    }
  }
}