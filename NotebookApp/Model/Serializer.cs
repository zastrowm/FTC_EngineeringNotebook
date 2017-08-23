using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace EngineeringNotebook.Model
{
  public class Serializer
  {
    public static void Serialize(PageEntryModel page, string filename)
    {
      XmlSerializer x = new XmlSerializer(typeof(PageEntryModel));
      using (TextWriter writer = new StreamWriter(filename))
      {
        x.Serialize(writer, page);
      }
    }

    public static PageEntryModel Deserialize(string filename)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(PageEntryModel));
      using (TextReader reader = new StreamReader(filename))
      {
        return (PageEntryModel)serializer.Deserialize(reader);
      }
    }
  }
}