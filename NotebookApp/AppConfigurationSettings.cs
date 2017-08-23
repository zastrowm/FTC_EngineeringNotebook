using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }

    public string[] Categories { get; }
    public string[] ProcessSteps { get; }
    public string[] Members { get; }
    public string[] CategoryColors { get; }
  }
}