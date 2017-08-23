using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EngineeringNotebook.Model;
using PropertyChanged;

namespace EngineeringNotebook.Mvvm
{
  [ImplementPropertyChanged]
  public class SubSectionEntryViewModel : BaseViewModel
  {
    public SubSectionEntryViewModel(SectionsViewModel owner)
    {
      Owner = owner;
      ListItems = new ListCollectionViewModel();
      AllImages = new ObservableCollection<ImageReference>()
                  {
                    new ImageReference(),
                  };

      AddImageCommand = new DelegateCommand(DoAddImage);
      RemoveImageCommand = new DelegateCommand<ImageReference>(DoRemove);

      AllImages.CollectionChanged += delegate { OnPropertyChanged(nameof(Image)); };

      MoveImageUpCommand = new DelegateCommand<ImageReference>(DoMoveImageUp);
      MoveImageDownCommand = new DelegateCommand<ImageReference>(DoMoveImageDown);
    }

    public DelegateCommand<ImageReference> MoveImageUpCommand { get; }
    public DelegateCommand<ImageReference> MoveImageDownCommand { get; }

    public SectionsViewModel Owner { get; }
    public string TextContent { get; set; }

    public string Title { get; set; }

    public ImageReference Image
      => AllImages.FirstOrDefault();

    public SubSectionType SectionType { get; set; }

    public ObservableCollection<ImageReference> AllImages { get; }

    public ListCollectionViewModel ListItems { get; }

    public DelegateCommand AddImageCommand { get; }

    public DelegateCommand<ImageReference> RemoveImageCommand { get; }

    private void DoAddImage()
    {
      AllImages.Add(new ImageReference());
    }

    private void DoRemove(ImageReference image)
    {
      var index = AllImages.IndexOf(image);

      if (index == 0)
      {
        Image.Image = default(ImageData);
      }
      else if (index != -1)
      {
        AllImages.RemoveAt(index);
      }
    }

    private void DoMoveImageUp(ImageReference image)
    {
      var index = AllImages.IndexOf(image);
      if (index <= 0)
        return;

      AllImages.Move(index, index - 1);
    }

    private void DoMoveImageDown(ImageReference image)
    {
      var index = AllImages.IndexOf(image);
      if (index >= AllImages.Count - 1)
        return;

      AllImages.Move(index, index + 1);
    }

    public SectionModel ToModel()
    {
      return new SectionModel()
             {
               Title = Title,
               TextContent = TextContent,
               ListItems = ListItems.GetModel(),
               Type = SectionType,
               AllImages = AllImages.Select(i => i.ToModel()).ToArray()
             };
    }

    public void LoadModel(SectionModel model)
    {
      Title = model.Title;
      TextContent = model.TextContent;
      ListItems.LodeModel(model.ListItems);
      SectionType = model.Type;

      AllImages.Clear();

      foreach (var imageModel in model.AllImages)
      {
        AllImages.Add(ImageReference.FromModel(imageModel));
      }

      if (AllImages.Count < 0)
      {
        AllImages.Add(new ImageReference());
      }
    }
  }
}