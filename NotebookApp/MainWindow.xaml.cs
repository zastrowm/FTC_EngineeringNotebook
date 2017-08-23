using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
using EngineeringNotebook.Model;
using EngineeringNotebook.Mvvm;
using NotebookApp.Tools;
using NotebookApp.Views;
using Control = System.Windows.Controls.Control;
using MessageBox = System.Windows.MessageBox;

namespace NotebookApp
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private string _currentFileName;
    private PageEntryViewModel _viewModel;
    private bool _isAllowedToClose;

    public MainWindow()
    {
      New_Execute(null, null);

      InitializeComponent();

      Closing += async delegate(object sender, CancelEventArgs e)
                 {
                   if (_isAllowedToClose)
                   {
                     App.Current.Shutdown();
                     return;
                   }

                   e.Cancel = true;
                   await Task.Yield();
                   Close_Execute(null, null);
                 };

      Loaded += async delegate
                {
                  try
                  {
                    while (true)
                    {
                      try
                      {
                        await Task.Delay(TimeSpan.FromSeconds(30));
                        var modelToSave = _viewModel.Save();
                        Serializer.Serialize(modelToSave, "session.engpage");
                      }
                      catch
                      {
                        // stay in loop
                      }
                    }
                  }
                  catch
                  {
                    // just keep going
                  }
                };
    }

    private static void SaveFile(string tempFile, FixedDocument fixedDocument)
    {
      using (var memoryStream = new MemoryStream())
      using (Package container = Package.Open(tempFile, FileMode.Create))
      {
        using (XpsDocument xpsDoc = new XpsDocument(container, CompressionOption.Maximum))
        {
          XpsSerializationManager rsm = new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);
          rsm.SaveAsXaml(fixedDocument);
        }
        memoryStream.Position = 0;
      }
    }

    public string CurrentFileName
    {
      get { return _currentFileName; }
      set
      {
        _currentFileName = value;
        Title = $"Engineering Notebook - {_currentFileName}";
      }
    }

    private void Save_Execute(object sender, ExecutedRoutedEventArgs e)
    {
      SaveFile();
    }

    private bool SaveFile()
    {
      FlickerFocusToForceBindingCommit();

      if (CurrentFileName == null)
      {
        var saveDialog = new SaveFileDialog();
        LoadFilters(saveDialog);
        saveDialog.FileName =
          $"{_viewModel.EntryDate:yy-MM-dd ddd hh.mm}-{_viewModel.AttendanceViewModel?.Author?.DisplayName}-.engpage";
        if (System.Windows.Forms.DialogResult.OK != saveDialog.ShowDialog())
          return false;

        CurrentFileName = saveDialog.FileName;
      }

      var modelToSave = _viewModel.Save();
      Serializer.Serialize(modelToSave, CurrentFileName);
      return true;
    }

    private void FlickerFocusToForceBindingCommit()
    {
      Control currentControl = Keyboard.FocusedElement as Control;

      if (currentControl != null)
      {
        // Force focus away from the current control to update its binding source.
        currentControl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        currentControl.Focus();
        // moving next is not enough, as the next item could be a menu item (in a different focus
        // scope) so also try the previous command. 
        currentControl.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
        currentControl.Focus();
      }
    }

    private void SaveAs_Execute(object sender, ExecutedRoutedEventArgs e)
    {
      var saveDialog = new SaveFileDialog();
      LoadFilters(saveDialog);
      if (saveDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        return;

      CurrentFileName = saveDialog.FileName;

      SaveFile();
    }

    private void LoadFilters(FileDialog fileDialog)
    {
      fileDialog.Filter = "Engineering Notebook Page|*.engpage";
      fileDialog.FileName = CurrentFileName;

      if (CurrentFileName != null)
      {
        fileDialog.InitialDirectory = Path.GetDirectoryName(CurrentFileName);
      }
    }

    private void Open_Execute(object sender, ExecutedRoutedEventArgs e)
    {
      var openDialog = new OpenFileDialog();
      LoadFilters(openDialog);
      if (openDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
      {
        LoadFileAt(openDialog.FileName);
      }
    }

    private void LoadFileAt(string filename)
    {
      PageEntryModel model;

      if (File.Exists(filename))
      {
        model = Serializer.Deserialize(filename);
      }
      else
      {
        model = new PageEntryModel();
      }

      SetAsViewModel(filename, model);
    }

    private void SetAsViewModel(string filename, PageEntryModel model)
    {
      CurrentFileName = filename;
      _viewModel = new PageEntryViewModel();
      _viewModel.Load(model);
      DataContext = _viewModel;
    }

    private void New_Execute(object sender, ExecutedRoutedEventArgs e)
    {
      bool shouldSetNewModel = false;

      if (_viewModel?.Title != null)
      {
        var result = MessageBox.Show("Would you like to save your file before creating a new entry?",
                                     "Save before new?",
                                     MessageBoxButton.YesNoCancel);

        if (result == MessageBoxResult.Yes && SaveFile() || result == MessageBoxResult.No)
        {
          shouldSetNewModel = true;
        }
      }
      else
      {
        shouldSetNewModel = true;
      }

      if (shouldSetNewModel)
      {
        var newModel = new PageEntryModel();

        if (_viewModel != null)
        {
          newModel.EntryDate = _viewModel.EntryDate;
        }

        SetAsViewModel(null, newModel);
      }
    }

    private void Close_Execute(object sender, ExecutedRoutedEventArgs e)
    {
      var result = MessageBox.Show("Would you like to save your file before exiting?",
                                   "Save before exit?",
                                   MessageBoxButton.YesNoCancel);

      if (result == MessageBoxResult.Yes)
      {
        if (SaveFile())
        {
          StartClosing();
        }
      }
      else if (result == MessageBoxResult.No)
      {
        StartClosing();
      }
      else
      {
        // Cancelled: NOOP
      }
    }

    private void StartClosing()
    {
      _isAllowedToClose = true;
      Close();
    }

    private void Print_Execute(object sender, ExecutedRoutedEventArgs e)
    {
      FlickerFocusToForceBindingCommit();
      var title = "PRINT PREVIEW";

      DoPrint(title);
    }

    private void BackConvert_Clicked(object sender, RoutedEventArgs e)
    {
      new BatchProcessFiles().ShowDialog();
    }

    private void PrintWithTitle_Clicked(object sender, RoutedEventArgs e)
    {
      string title = Microsoft.VisualBasic.Interaction.InputBox("Enter Print Title",
                                                                "Enter Practice Name",
                                                                "Practice #");

      DoPrint(title);
    }

    private void DoPrint(string title)
    {
      try
      {
        var fixedDocument = new FixedDocument();

        var creator = new PageCreator(_viewModel, title);
        var pages = creator.CreatePages();

        foreach (var page in pages)
        {
          fixedDocument.Pages.Add(page);
        }

        string tempFileName = Path.GetTempFileName();
        SaveFile(tempFileName, fixedDocument);

        var tempDoc = new XpsDocument(tempFileName, FileAccess.Read);
        new Window()
        {
          Content = new DocumentViewer()
                    {
                      Document = tempDoc.GetFixedDocumentSequence(),
                    },
          Owner = this,
        }.ShowDialog();
        tempDoc.Close();

        File.Delete(tempFileName);
      }
      catch (Exception exception)
      {
        MessageBox.Show(exception.ToString(), "Error Occurred");
      }
    }

    private void Help_Execute(object sender, ExecutedRoutedEventArgs e)
    {
      new AppHelp().ShowDialog();
    }
  }
}