using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using EngineeringNotebook.Mvvm;

namespace EngineeringNotebook.Converters
{
  public class ImageBytesConverter : BaseOneWayConverter<ImageData, BitmapSource>
  {
    public override BitmapSource ConvertValue(ImageData fromValue, object parameter, CultureInfo language)
    {
      if (fromValue.Data == null)
        return null;

      var memoryStream = new MemoryStream(fromValue.Data);
      var imageSource = new BitmapImage();
      imageSource.BeginInit();
      imageSource.StreamSource = memoryStream;
      imageSource.EndInit();

      // Assign the Source property of your image
      return imageSource;
    }
  }
}