using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using EngineeringNotebook.Model;
using NotebookApp;
using PropertyChanged;

namespace EngineeringNotebook.Mvvm
{
  [ImplementPropertyChanged]
  public class PageEntryViewModel : BaseViewModel
  {
    public PageEntryViewModel()
    {
      ItemsAccomplished = new ListCollectionViewModel();
      NextSteps = new ListCollectionViewModel();
      AttendanceViewModel = new AttendanceViewModel();
      Sections = new SectionsViewModel();

      Category = Category.AvailableCategories.FirstOrDefault();

      ProcessSteps = new BindingList<ProcessStep>(
        AppConfigurationSettings.Instance.ProcessSteps
                                .Select(s => new ProcessStep(s))
                                .ToList()
      );

      ProcessSteps.ListChanged += delegate { OnPropertyChanged(nameof(ProcessSteps)); };
    }

    public Category Category { get; set; }

    public BindingList<ProcessStep> ProcessSteps { get; set; }

    /// <summary> The title of the entry. </summary>
    public string Title { get; set; }

    public DateTime EntryDate { get; set; }

    public ListCollectionViewModel ItemsAccomplished { get; }
    public ListCollectionViewModel NextSteps { get; }

    public SectionsViewModel Sections { get; }

    public AttendanceViewModel AttendanceViewModel { get; }

    public string Reflections { get; set; }

    public PageEntryViewModel Load(PageEntryModel model)
    {
      Title = model.Title;
      EntryDate = model.EntryDate ?? DateTime.Now;

      if (model.Category != null)
      {
        model.Category = ModifyCategory(model.Category);

        Category = Category.AvailableCategories
                           .FirstOrDefault(c => c.DisplayName == model.Category);

        if (Category == null)
        {
          Category = Category.AvailableCategories.FirstOrDefault(c => c.DisplayName == model.Category);
        }
      }

      if (model.Steps != null)
      {
        foreach (var step in model.Steps)
        {
          var stepVm = FindStep(step);
          if (stepVm != null)
          {
            stepVm.IsPresent = true;
          }
        }
      }

      ItemsAccomplished.LodeModel(model.ItemsAccomplished);
      NextSteps.LodeModel(model.NextSteps);
      Sections.LoadModel(model.Sections);

      AttendanceViewModel.SetAuthorAndAttendees(model.Author, model.Attended);

      Reflections = model.Reflections;

      return this;
    }

    private string ModifyCategory(string modelCategory)
    {
      switch (modelCategory)
      {
        case "Harvester":
        case "Shooter":
          return "Particle System";
        case "CapBall & Beacons":
          return "Beacon System";
      }

      return modelCategory;
    }

    private ProcessStep FindStep(string name)
    {
      return ProcessSteps.FirstOrDefault(a => a.DisplayName == name);
    }

    public PageEntryModel Save()
    {
      var model = new PageEntryModel
                  {
                    Title = Title,
                    Category = Category.DisplayName,
                    Steps = ProcessSteps.Where(s => s.IsPresent).Select(s => s.DisplayName).ToArray(),
                    EntryDate = EntryDate,
                    NextSteps = NextSteps.GetModel(),
                    ItemsAccomplished = ItemsAccomplished.GetModel(),
                    Attended = AttendanceViewModel.GetModel(),
                    Author = AttendanceViewModel.Author?.DisplayName,
                    Sections = Sections.GetModel(),
                    Reflections = Reflections,
                  };

      return model;
    }

    public PageEntryViewModel Clone()
    {
      var clone = new PageEntryViewModel();
      var data = Save();
      clone.Load(data);
      return clone;
    }
  }
}