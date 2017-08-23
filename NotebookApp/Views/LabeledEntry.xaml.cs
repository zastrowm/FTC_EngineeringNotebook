using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace EngineeringNotebook
{
  /// <summary>
  /// Interaction logic for LabeledEntry.xaml
  /// </summary>
  [ContentProperty("Editor")]
  public partial class LabeledEntry : UserControl
  {
    public LabeledEntry()
    {
      InitializeComponent();
    }

    public static readonly DependencyProperty EditorProperty = DependencyProperty.Register(
      "Editor",
      typeof(object),
      typeof(LabeledEntry),
      new PropertyMetadata(default(object)));

    public object Editor
    {
      get { return (object)GetValue(EditorProperty); }
      set { SetValue(EditorProperty, value); }
    }

    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
      "Title",
      typeof(string),
      typeof(LabeledEntry),
      new PropertyMetadata(default(string)));

    public string Title
    {
      get { return (string)GetValue(TitleProperty); }
      set { SetValue(TitleProperty, value); }
    }
  }
}