using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NotebookApp.Printing
{
  public class CategoryItem : Freezable
  {
    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
      "Text",
      typeof(string),
      typeof(CategoryItem),
      new PropertyMetadata(default(string)));

    public string Text
    {
      get { return (string)GetValue(TextProperty); }
      set { SetValue(TextProperty, value); }
    }

    public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
      "Color",
      typeof(Color),
      typeof(CategoryItem),
      new PropertyMetadata(default(Color)));

    public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected",
                                                                                               typeof(bool),
                                                                                               typeof(CategoryItem),
                                                                                               new PropertyMetadata(
                                                                                                 default(bool)));

    public Color Color
    {
      get { return (Color)GetValue(ColorProperty); }
      set { SetValue(ColorProperty, value); }
    }

    public bool IsSelected
    {
      get { return (bool)GetValue(IsSelectedProperty); }
      set { SetValue(IsSelectedProperty, value); }
    }

    protected override Freezable CreateInstanceCore()
    {
      return new CategoryItem();
    }
  }
}