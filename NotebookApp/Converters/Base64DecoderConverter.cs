using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EngineeringNotebook.Converters;

namespace NotebookApp.Converters
{
  public class Base64DecoderConverter : BaseOneWayConverter<string, string>
  {
    public override string ConvertValue(string fromValue, object parameter, CultureInfo language)
    {
      byte[] data = System.Convert.FromBase64String(fromValue);
      return Encoding.UTF8.GetString(data);
    }
  }
}