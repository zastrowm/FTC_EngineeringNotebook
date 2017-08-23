using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PropertyChanged;

namespace EngineeringNotebook.Mvvm
{
  [ImplementPropertyChanged]
  public abstract class BaseCollectionViewModel<T> : BaseViewModel
  {
    public ObservableCollection<T> Items { get; }
    = new ObservableCollection<T>();

    public BaseCollectionViewModel()
    {
      RemoveItemCommand = new DelegateCommand<T>(DoRemoveItem, CanRemoveItem);
      AddItemCommand = new DelegateCommand(DoAddItem);

      MoveItemUpCommand = new DelegateCommand<T>(DoMoveItemUp, CanMoveItemUp);
      MoveItemDownCommand = new DelegateCommand<T>(DoMoveItemDown, CanMoveItemDown);

      Items.CollectionChanged += delegate
                                 {
                                   RemoveItemCommand.RaiseCanExecuteChanged();
                                   MoveItemDownCommand.RaiseCanExecuteChanged();
                                   MoveItemUpCommand.RaiseCanExecuteChanged();
                                 };
    }

    private bool CanMoveItemDown(T instance)
      => Items.IndexOf(instance) < Items.Count - 1;

    private void DoMoveItemDown(T instance)
    {
      var index = Items.IndexOf(instance);
      Items.Move(index, index + 1);
    }

    private bool CanMoveItemUp(T instance)
      => Items.IndexOf(instance) > 0;

    private void DoMoveItemUp(T instance)
    {
      var index = Items.IndexOf(instance);
      Items.Move(index, index - 1);
    }

    protected virtual bool CanRemoveItem(T instance)
      => Items.Count > 0;

    protected virtual void DoRemoveItem(T instance)
    {
      Items.Remove(instance);
    }

    public DelegateCommand<T> RemoveItemCommand { get; }

    public DelegateCommand AddItemCommand { get; }

    public DelegateCommand<T> MoveItemUpCommand { get; }
    public DelegateCommand<T> MoveItemDownCommand { get; }

    protected virtual void DoAddItem()
      => Items.Add(CreateInstance());

    protected abstract T CreateInstance();
  }
}