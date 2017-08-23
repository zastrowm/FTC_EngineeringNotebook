using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EngineeringNotebook.Model;
using EngineeringNotebook.Mvvm;
using NotebookApp.Printing;

namespace NotebookApp
{
  public static class MockData
  {
    static MockData()
    {
      try
      {
        var data = Serializer.Deserialize(@"T:\blueprint.engpage");
        var vm = new PageEntryViewModel();
        vm.Load(data);
        Page = vm;

        PrintedPage = new PrintedPageData()
                      {
                        PageData = Page,
                        ShowCategories = true,
                        ShowFooter = false,
                      };
      }
      catch (Exception)
      {
        // no problem, just mock data anyways
      }
    }

    public static PageEntryViewModel Page { get; }

    public static PrintedPageData PrintedPage { get; }
  }
}