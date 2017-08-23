using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace EngineeringNotebook.Converters
{
  /// <summary> Returns true if the value is equal to the parameter, false otherwise. </summary>
  public class IsEqualToConverter : MarkupExtension,
                                    IMultiValueConverter
  {
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
      return Equals(values[0], values[1]);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
      return null;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return new IsEqualToConverter();
    }
  }
}