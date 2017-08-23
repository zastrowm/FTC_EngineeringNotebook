using System;
using System.Collections.Generic;
using System.Linq;
using PropertyChanged;

namespace EngineeringNotebook.Mvvm
{
  [ImplementPropertyChanged]
  public class ProcessStep : BaseViewModel
  {
    public ProcessStep(string displayName)
    {
      DisplayName = displayName;
    }

    public bool IsPresent { get; set; }

    /// <summary> The display name of the category.  </summary>
    public string DisplayName { get; }
  }
}