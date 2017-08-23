using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using NotebookApp;

namespace EngineeringNotebook.Mvvm
{
  /// <summary> A category. </summary>
  public class Category
  {
    public static ObservableCollection<Category> AvailableCategories { get; }
      = new ObservableCollection<Category>(
        Enumerable.Zip(
          AppConfigurationSettings.Instance.Categories,
          AppConfigurationSettings.Instance.CategoryColors,
          (category, color) => new Category(category, (Color)ColorConverter.ConvertFromString(color))
        )
      );

    public Category(string displayName, Color color)
    {
      DisplayName = displayName;
      Color = color;
    }

    /// <summary> The display name of the category.  </summary>
    public string DisplayName { get; }

    public Color Color { get; }
  }
}