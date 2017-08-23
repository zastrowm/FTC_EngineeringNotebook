using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngineeringNotebook.Mvvm;
using PropertyChanged;

namespace NotebookApp.Printing
{
  [ImplementPropertyChanged]
  public class PrintedPageData : BaseViewModel
  {
    public bool ShowCategories { get; set; }

    public bool ShowSteps { get; set; }

    public bool ShowFooter { get; set; }

    public PageEntryViewModel PageData { get; set; }
  }
}