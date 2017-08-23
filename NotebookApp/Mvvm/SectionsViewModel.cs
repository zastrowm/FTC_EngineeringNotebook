using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EngineeringNotebook.Model;
using PropertyChanged;

namespace EngineeringNotebook.Mvvm
{
  [ImplementPropertyChanged]
  public class SectionsViewModel : BaseViewModel
  {
    public SectionsViewModel()
    {
      Items = new ObservableCollection<SubSectionEntryViewModel>()
              {
                new SubSectionEntryViewModel(this)
                {
                  SectionType = SubSectionType.ImageCollection,
                },
                new SubSectionEntryViewModel(this)
                {
                  Title = "Additional Notes",
                  SectionType = SubSectionType.Title,
                },
                new SubSectionEntryViewModel(this)
                {
                  TextContent = "Additional Notes",
                  SectionType = SubSectionType.Text
                },
                new SubSectionEntryViewModel(this)
                {
                  SectionType = SubSectionType.ImageAndCaption,
                  Title = "An image alongside what should be done",
                },
                new SubSectionEntryViewModel(this)
                {
                  SectionType = SubSectionType.ImageWithCaptionAndBlurb,
                  Title = "An image alongside what should be done",
                  TextContent = "An example of image data",
                },
              };

      AddSectionCommand = new DelegateCommand<SubSectionType>(DoAddSection);
      MoveSectionUpCommand = new DelegateCommand<SubSectionEntryViewModel>(DoMoveSectionUp, CanMoveSectionUp);
      MoveSectionDownCommand = new DelegateCommand<SubSectionEntryViewModel>(DoMoveSectionDown, CanMoveSectionDown);
      RemoveSectionCommand = new DelegateCommand<SubSectionEntryViewModel>(DoRemoveSection);

      Items.CollectionChanged += delegate
                                 {
                                   MoveSectionDownCommand.RaiseCanExecuteChanged();
                                   MoveSectionUpCommand.RaiseCanExecuteChanged();
                                 };
    }

    public ObservableCollection<SubSectionEntryViewModel> Items { get; }

    public DelegateCommand<SubSectionType> AddSectionCommand { get; }

    public DelegateCommand<SubSectionEntryViewModel> MoveSectionUpCommand { get; }
    public DelegateCommand<SubSectionEntryViewModel> MoveSectionDownCommand { get; }
    public DelegateCommand<SubSectionEntryViewModel> RemoveSectionCommand { get; }

    private bool CanMoveSectionUp(SubSectionEntryViewModel section)
    {
      var index = Items.IndexOf(section);
      return index > 0;
    }

    private void DoMoveSectionUp(SubSectionEntryViewModel section)
    {
      var index = Items.IndexOf(section);
      Items.Move(index, index - 1);
    }

    private bool CanMoveSectionDown(SubSectionEntryViewModel section)
    {
      var index = Items.IndexOf(section);
      return index < Items.Count - 1;
    }

    private void DoMoveSectionDown(SubSectionEntryViewModel section)
    {
      var index = Items.IndexOf(section);
      Items.Move(index, index + 1);
    }

    private void DoRemoveSection(SubSectionEntryViewModel section)
    {
      Items.Remove(section);
    }

    private void DoAddSection(SubSectionType type)
    {
      Items.Add(new SubSectionEntryViewModel(this)
                {
                  SectionType = type,
                });
    }

    public SectionModel[] GetModel()
    {
      return Items.Select(s => s.ToModel()).ToArray();
    }

    public void LoadModel(SectionModel[] sectionModels)
    {
      Items.Clear();

      if (sectionModels == null)
        return;

      foreach (var sectionModel in sectionModels)
      {
        var section = new SubSectionEntryViewModel(this);
        section.LoadModel(sectionModel);
        Items.Add(section);
      }
    }
  }
}