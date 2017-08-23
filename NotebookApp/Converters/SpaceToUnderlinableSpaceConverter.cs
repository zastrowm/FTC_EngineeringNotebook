using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace EngineeringNotebook.Converters
{
  public class SpaceToUnderlinableSpaceConverter : BaseOneWayConverter<string, string>
  {
    public override string ConvertValue(string fromValue, object parameter, CultureInfo language)
    {
      return fromValue.Replace(" ", "\u00a0");
    }
  }
}