using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EngineeringNotebook.Mvvm;

namespace EngineeringNotebook.Converters
{
  public class SubSectionTypeConverter : BaseOneWayConverter<SubSectionType, string>
  {
    public override string ConvertValue(SubSectionType fromValue, object parameter, CultureInfo language)
    {
      switch (fromValue)
      {
        case SubSectionType.Title:
          return "Section Title";
        case SubSectionType.ImageAndCaption:
          return "Image (Large)";
        case SubSectionType.ImageWithCaptionAndBlurb:
          return "Image & Blurb";
        case SubSectionType.ImageCollection:
          return "Image Collection";
        case SubSectionType.Text:
          return "Paragraph";
        case SubSectionType.ListItems:
          return "List";
        default:
          throw new ArgumentOutOfRangeException(nameof(fromValue), fromValue, null);
      }
    }
  }
}