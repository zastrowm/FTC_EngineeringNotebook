using System;
using System.Collections.Generic;
using System.Linq;
using EngineeringNotebook.Mvvm;

namespace EngineeringNotebook.Model
{
  [Serializable]
  public class PageEntryModel
  {
    public string Title { get; set; }
    public DateTime? EntryDate { get; set; }
    public string Category { get; set; }
    public string[] Steps { get; set; }
    public string[] ItemsAccomplished { get; set; }
    public string[] NextSteps { get; set; }
    public string[] Attended { get; set; }
    public string Author { get; set; }
    public AdditionalNote[] AdditionalNotes { get; set; }
    public SectionModel[] Sections { get; set; }
    public string Reflections { get; set; }
  }

  public class AdditionalNote
  {
    public string Title { get; set; }
    public string Content { get; set; }
  }

  public class ImageModel
  {
    public string Base64ImageData { get; set; }
  }

  public class SectionModel
  {
    public string TextContent { get; set; }
    public string Title { get; set; }
    public ImageModel[] AllImages { get; set; }
    public string[] ListItems { get; set; }
    public SubSectionType Type { get; set; }
  }
}