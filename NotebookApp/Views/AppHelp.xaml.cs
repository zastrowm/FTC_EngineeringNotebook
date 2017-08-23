using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using NotebookApp.Converters;
using NotebookApp.Properties;

namespace NotebookApp.Views
{
  /// <summary>
  /// Interaction logic for AppHelp.xaml
  /// </summary>
  public partial class AppHelp : Window
  {
    public AppHelp()
    {
      InitializeComponent();
    }

    private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
    {
      var email = new Base64DecoderConverter().ConvertValue(AppVersionInfo.EmailBase64, null, null);
      Clipboard.SetText(email);

      Process.Start($"mailto:{email}");
    }
  }
}