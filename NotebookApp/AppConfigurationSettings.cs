using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NotebookApp
{
  public class AppConfigurationSettings
  {
    public static AppConfigurationSettings Instance { get; private set; }

    public static Exception Load(string file)
    {
      try
      {
        Instance = new AppConfigurationSettings(file);
        return null;
      }
      catch (Exception e)
      {
        return e;
      }
    }

    public AppConfigurationSettings(string file)
    {
      var doc = XDocument.Load(file);

      Categories = doc.XPathSelectElements("//Configuration/Categories/Category")
                      .Select(it => it.Value)
                      .ToArray();

      ProcessSteps = doc.XPathSelectElements("//Configuration/Steps/Step")
                        .Select(it => it.Value)
                        .ToArray();

      Members = doc.XPathSelectElements("//Configuration/Members/Member")
                   .Select(it => it.Value)
                   .ToArray();

      CategoryColors = doc.XPathSelectElements("//Configuration/Theme/CategoryColor")
                          .Select(it => it.Value)
                          .ToArray();

      PrimaryColor = doc.XPathSelectElement("//Configuration/Theme/PrimaryColor")?.Value
        ?? "#FF99A1";

      SecondaryColor = doc.XPathSelectElement("//Configuration/Theme/SecondaryColor")?.Value
        ?? "#FFD6D9";
    }

    public string[] Categories { get; }
    public string[] ProcessSteps { get; }
    public string[] Members { get; }
    public string[] CategoryColors { get; }
    public string PrimaryColor { get; }
    public string SecondaryColor { get; }

    public ImageSource Image
    {
      get
      {
        try
        {
          string path = Path.Combine(Environment.CurrentDirectory, @"image.png");
          if (File.Exists(path))
          {
            return new BitmapImage(new Uri(path, UriKind.Absolute));   
          }
        }
        catch
        {
        }

        return DefaultImageSource;
      }
    }

    private static ImageSource DefaultImageSource
      = LoadDefaultImage();

    private static ImageSource LoadDefaultImage()
    {
      using (Stream memoryStream = App.GetDefaultImagePng())
      {
        var imageSource = new BitmapImage();
        imageSource.BeginInit();
        imageSource.StreamSource = memoryStream;
        imageSource.EndInit();
        return imageSource;
      }
    }
  }
}