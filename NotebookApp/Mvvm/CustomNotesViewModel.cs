using System;
using System.Collections.Generic;
using System.Linq;
using PropertyChanged;

namespace EngineeringNotebook.Mvvm
{
  [ImplementPropertyChanged]
  public class CustomNotesViewModel : BaseViewModel
  {
    public string Heading { get; set; }
    = "More Notes";

    /// <summary> The content of the summary. </summary>
    public string Content { get; set; }
  }
}