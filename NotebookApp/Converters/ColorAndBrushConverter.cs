using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using EngineeringNotebook.Converters;

namespace NotebookApp.Converters
{
  public class ColorAndBrushConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      var color = (Color)ColorConverter.ConvertFromString((string)value);

      if (targetType == typeof(Color))
      {
        return color;
      }

      if (targetType == typeof(Brush))
      {
        return new SolidColorBrush(color);
      }

      return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      => DependencyProperty.UnsetValue;
  }
}
