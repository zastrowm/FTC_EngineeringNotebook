using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using EngineeringNotebook.Model;
using PropertyChanged;

namespace EngineeringNotebook.Mvvm
{
  [ImplementPropertyChanged]
  public class ImageReference
  {
    public ImageData Image { get; set; }

    static ImageReference()
    {
      RotateImageRightCommand = new DelegateCommand<ImageReference>(DoRotateImageRight);
      RotateImageLeftCommand = new DelegateCommand<ImageReference>(DoRotateImageLeft);
    }

    public static DelegateCommand<ImageReference> RotateImageRightCommand { get; }

    public static DelegateCommand<ImageReference> RotateImageLeftCommand { get; }

    public ImageModel ToModel()
    {
      return new ImageModel()
             {
               Base64ImageData = Image.IsEmpty ? null : Convert.ToBase64String(Image.Data)
             };
    }

    public static ImageReference FromModel(ImageModel model)
    {
      return new ImageReference()
             {
               Image = string.IsNullOrWhiteSpace(model.Base64ImageData)
                 ? default(ImageData)
                 : new ImageData(0, 0, Convert.FromBase64String(model.Base64ImageData))
             };
    }

    private static void DoRotateImageRight(ImageReference image)
    {
      using (var newStream = new MemoryStream())
      using (var stream = new MemoryStream(image.Image.Data))
      using (var bitmap = new Bitmap(stream))
      {
        bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
        bitmap.Save(newStream, System.Drawing.Imaging.ImageFormat.Png);

        image.Image = new ImageData(bitmap.Width, bitmap.Height, newStream.ToArray());
      }
    }

    private static void DoRotateImageLeft(ImageReference image)
    {
      using (var newStream = new MemoryStream())
      using (var stream = new MemoryStream(image.Image.Data))
      using (var bitmap = new Bitmap(stream))
      {
        bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
        bitmap.Save(newStream, System.Drawing.Imaging.ImageFormat.Png);

        image.Image = new ImageData(bitmap.Width, bitmap.Height, newStream.ToArray());
      }
    }
  }
}