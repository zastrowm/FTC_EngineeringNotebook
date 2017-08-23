using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EngineeringNotebook.Mvvm;
using TakeSnapsWithWebcamUsingWpfMvvm.Video;

namespace EngineeringNotebook.Views
{
  /// <summary>
  /// Interaction logic for CameraSnapshot.xaml
  /// </summary>
  public partial class CameraSnapshot : UserControl
  {
    public static readonly DependencyProperty CancelledCommandProperty = DependencyProperty.Register(
      "CancelledCommand",
      typeof(ICommand),
      typeof(CameraSnapshot),
      new PropertyMetadata(default(ICommand)));

    public static readonly DependencyProperty AcceptedCommandProperty = DependencyProperty.Register(
      "AcceptedCommand",
      typeof(DelegateCommand<ImageData>),
      typeof(CameraSnapshot),
      new PropertyMetadata(default(DelegateCommand<ImageData>)));

    public static readonly DependencyProperty SelectedDeviceProperty = DependencyProperty.Register(
      "SelectedDevice",
      typeof(MediaInformation),
      typeof(CameraSnapshot),
      new PropertyMetadata(default(MediaInformation), OnSelectedDeviceChanged));

    public CameraSnapshot()
    {
      InitializeComponent();

      if (AvailableDevices.Count == 0)
      {
        foreach (var devices in WebcamDevice.VideoDevices)
        {
          AvailableDevices.Add(devices);
        }
      }

      SelectedDevice = AvailableDevices.LastOrDefault(id => SelectedDeviceId == id.UsbId)
                       ?? AvailableDevices.LastOrDefault();

      Unloaded += delegate
                  {
                    SelectedDeviceId = SelectedDevice?.UsbId;
                    Webcam.VideoSourceId = "";
                  };
    }

    public static ObservableCollection<MediaInformation> AvailableDevices { get; }
    = new ObservableCollection<MediaInformation>();

    public static string SelectedDeviceId { get; private set; }

    public MediaInformation SelectedDevice
    {
      get { return (MediaInformation)GetValue(SelectedDeviceProperty); }
      set { SetValue(SelectedDeviceProperty, value); }
    }

    public ImageData SnapshottedImage { get; private set; }

    public DelegateCommand<ImageData> AcceptedCommand
    {
      get { return (DelegateCommand<ImageData>)GetValue(AcceptedCommandProperty); }
      set { SetValue(AcceptedCommandProperty, value); }
    }

    public ICommand CancelledCommand
    {
      get { return (ICommand)GetValue(CancelledCommandProperty); }
      set { SetValue(CancelledCommandProperty, value); }
    }

    public void Shutdown()
    {
      Webcam.Shutdown();
    }

    private void HandleSelectedDeviceChange()
    {
      Webcam.VideoSourceId = SelectedDevice.UsbId;
    }

    private static void OnSelectedDeviceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      ((CameraSnapshot)d).HandleSelectedDeviceChange();
    }

    private void HandleTakePicture(object sender, RoutedEventArgs e)
    {
      var frame = Webcam.GetFrame();
      AcceptedCommand.TryExecute(ConvertToImageBytes(frame));
    }

    public static ImageData ConvertToImageBytes(Bitmap bitmap)
    {
      byte[] byteArray;
      using (var stream = new MemoryStream())
      {
        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        byteArray = stream.ToArray();
      }

      return new ImageData(bitmap.Width, bitmap.Height, byteArray);
    }
  }
}