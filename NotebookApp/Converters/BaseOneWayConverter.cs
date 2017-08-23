using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace EngineeringNotebook.Converters
{
  public abstract class BaseOneWayConverter<TFrom, TTo> : BaseMarkupExtension,
                                                          IValueConverter
  {
    public abstract TTo ConvertValue(TFrom fromValue, object parameter, CultureInfo language);

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
      => ConvertValue((TFrom)value, parameter, culture);

    public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
      => DependencyProperty.UnsetValue;
  }
}