using System;
using System.Collections.Generic;
using System.Linq;
using PropertyChanged;

namespace EngineeringNotebook.Mvvm
{
  [ImplementPropertyChanged]
  public class ListCollectionViewModel : BaseCollectionViewModel<ListItemViewModel>
  {
    public ListCollectionViewModel()
    {
      Items.Add(new ListItemViewModel(this));
    }

    protected override bool CanRemoveItem(ListItemViewModel instance)
      => Items.Count > 1;

    protected override ListItemViewModel CreateInstance()
    {
      return new ListItemViewModel(this);
    }

    public void LodeModel(string[] items)
    {
      if (items == null)
        return;

      Items.Clear();
      foreach (var item in items)
      {
        Items.Add(new ListItemViewModel(this) { Content = item });
      }
    }

    public string[] GetModel()
    {
      return Items
        .Select(s => s.Content)
        .Where(s => !string.IsNullOrEmpty(s))
        .ToArray();
    }
  }

  public class ListItemViewModel : BaseViewModel
  {
    public ListCollectionViewModel Owner { get; }

    public ListItemViewModel(ListCollectionViewModel owner)
    {
      Owner = owner;
    }

    public string Content { get; set; }
  }
}