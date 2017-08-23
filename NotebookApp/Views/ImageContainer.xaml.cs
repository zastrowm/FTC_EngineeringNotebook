using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using EngineeringNotebook.Mvvm;
using EngineeringNotebook.Views;
using Microsoft.Win32;

namespace NotebookApp.Views
{
  /// <summary>
  /// Interaction logic for ImageContainer.xaml
  /// </summary>
  public partial class ImageContainer : UserControl
  {
    public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
      "Image",
      typeof(ImageData),
      typeof(ImageContainer),
      new FrameworkPropertyMetadata(default(ImageData))
      {
        BindsTwoWayByDefault = true,
      });

    public static readonly DependencyProperty ResetImageCommandProperty = DependencyProperty.Register(
      "ResetImageCommand",
      typeof(ICommand),
      typeof(ImageContainer),
      new PropertyMetadata(default(ICommand)));

    public static readonly DependencyProperty ResetImageCommandParameterProperty = DependencyProperty.Register(
      "ResetImageCommandParameter",
      typeof(object),
      typeof(ImageContainer),
      new PropertyMetadata(default(object)));

    private static CachedCameraTaker _pictureHelper;

    public ImageContainer()
    {
      InitializeComponent();

      if (_pictureHelper == null)
      {
        _pictureHelper = new CachedCameraTaker();
      }
    }

    public ImageData Image
    {
      get { return (ImageData)GetValue(ImageProperty); }
      set { SetValue(ImageProperty, value); }
    }

    public object ResetImageCommandParameter
    {
      get { return (object)GetValue(ResetImageCommandParameterProperty); }
      set { SetValue(ResetImageCommandParameterProperty, value); }
    }

    public ICommand ResetImageCommand
    {
      get { return (ICommand)GetValue(ResetImageCommandProperty); }
      set { SetValue(ResetImageCommandProperty, value); }
    }

    /// <summary> Takes a picture using the camera. </summary>
    private void HandleTakePictureClicked(object sender, RoutedEventArgs e)
    {
      Image = _pictureHelper.TakePicture(Window.GetWindow(this));
    }

    private static string LastFile;

    /// <summary> Pops up an OpenFile dialog for images. </summary>
    private void BrowseForPicture(object sender, RoutedEventArgs e)
    {
      var openDialog = new OpenFileDialog
                       {
                         Filter =
                           "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
                       };

      if (LastFile != null)
      {
        openDialog.InitialDirectory = Path.GetDirectoryName(LastFile);
      }


      if (openDialog.ShowDialog() != true)
        return;

      var theFile = openDialog.FileName;
      LastFile = openDialog.FileName;

      try
      {
        using (var bitmap = new Bitmap(theFile))
        {
          SetCurrentValue(ImageProperty, CameraSnapshot.ConvertToImageBytes(bitmap));
        }
      }
      catch (Exception exception)
      {
        MessageBox.Show(exception.ToString(), "Error");
      }
    }

    /// <summary> Hosts that control which takes a picture form the camera </summary>
    private class CachedCameraTaker
    {
      private CameraSnapshot _cameraSnapshot;
      private DateTime _lastStartTime;

      private bool _isUp;
      private readonly DispatcherTimer _timer;

      public CachedCameraTaker()
      {
        _timer = new DispatcherTimer();
        _timer.Interval = TimeSpan.FromSeconds(10);
        _timer.Tick += HandleTimerTick;
      }

      public ImageData TakePicture(Window owner)
      {
        var window = new Window
                     {
                       Owner = owner,
                       Style = (Style)owner.FindResource("PrimaryWindow"),
                     };

        if (_cameraSnapshot == null)
        {
          _cameraSnapshot = new CameraSnapshot();
        }

        var theImage = default(ImageData);

        _cameraSnapshot.AcceptedCommand = new DelegateCommand<ImageData>(data =>
                                                                         {
                                                                           theImage = data;
                                                                           window.Close();
                                                                         });
        _cameraSnapshot.CancelledCommand = new DelegateCommand(() => { window.Close(); });
        window.Content = _cameraSnapshot;

        _timer.Stop();
        _isUp = true;
        window.ShowDialog();
        _isUp = false;
        _timer.Start();

        return theImage;
      }

      private void HandleTimerTick(object sender, EventArgs e)
      {
        if (_isUp || _cameraSnapshot == null)
          return;

        _cameraSnapshot.Shutdown();
        _cameraSnapshot = null;
        _timer.Stop();
      }
    }

    private void ResetImage(object sender, RoutedEventArgs e)
    {
      SetCurrentValue(ImageProperty, default(ImageData));
    }
  }
}