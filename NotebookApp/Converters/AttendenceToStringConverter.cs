using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EngineeringNotebook.Mvvm;

namespace EngineeringNotebook.Converters
{
  public class AttendenceToStringConverter : BaseOneWayConverter<ICollection<Individual>, string>
  {
    public override string ConvertValue(ICollection<Individual> individuals, object parameter, CultureInfo language)
    {
      var names = individuals
        .Where(i => i.DidAttend)
        .Select(i => i.DisplayName);

      var content = string.Join(", ", names);

      if (content == "")
      {
        content = "<none selected>";
      }

      return content;
    }
  }

  public class ProcessSTepToStringConverter : BaseOneWayConverter<ICollection<ProcessStep>, string>
  {
    public override string ConvertValue(ICollection<ProcessStep> individuals, object parameter, CultureInfo language)
    {
      var names = individuals
        .Where(i => i.IsPresent)
        .Select(i => i.DisplayName);

      var content = string.Join(", ", names);

      if (content == "")
      {
        content = "<none selected>";
      }

      return content;
    }
  }
}