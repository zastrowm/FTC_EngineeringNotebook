using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Markup;

namespace EngineeringNotebook.Converters
{
  public abstract class BaseMarkupExtension : MarkupExtension
  {
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      return Activator.CreateInstance(GetType());
    }
  }
}