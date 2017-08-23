using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NotebookApp.Views;

namespace NotebookApp
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public const string DefaultAppConfigurationFilename = "app-configuration.xml";

    public static FileInfo FileLocation { get; }
      = new FileInfo(DefaultAppConfigurationFilename);

    protected override void OnStartup(StartupEventArgs e)
    {
      ShutdownMode = ShutdownMode.OnExplicitShutdown;

      var locationToCheck = FileLocation;

      if (!locationToCheck.Exists)
      {
        new CreatingDefaultConfigurationFileWindow().ShowDialog();

        using (var stream = GetDefaultAppConfigurationStream())
        using (var output = locationToCheck.OpenWrite())
        {
          stream.CopyTo(output);
        }
      }

      var exception = AppConfigurationSettings.Load(locationToCheck.FullName);

      if (exception != null)
      {
        MessageBox.Show(
          exception.ToString(),
          "Exception while loading app configuration settings");
        Shutdown();
        return;
      }

      MainWindow = new MainWindow();

      base.OnStartup(e);
    }

    private static Stream GetDefaultAppConfigurationStream()
    {
      return typeof(App).Assembly.GetManifestResourceStream("NotebookApp.app-configuration.xml");
    }
  }
}