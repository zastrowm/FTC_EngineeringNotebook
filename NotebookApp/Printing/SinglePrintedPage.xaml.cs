using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NotebookApp.Printing
{
  /// <summary>
  /// Interaction logic for SinglePrintedPage.xaml
  /// </summary>
  public partial class SinglePrintedPage : UserControl
  {
    public static readonly DependencyProperty PageNumberProperty = DependencyProperty.Register(
      "PageNumber",
      typeof(int),
      typeof(SinglePrintedPage),
      new PropertyMetadata(default(int)));

    public int PageNumber
    {
      get { return (int)GetValue(PageNumberProperty); }
      set { SetValue(PageNumberProperty, value); }
    }

    public static readonly DependencyProperty TotalNumberOfPagesProperty = DependencyProperty.Register(
      "TotalNumberOfPages",
      typeof(int),
      typeof(SinglePrintedPage),
      new PropertyMetadata(default(int)));

    public int TotalNumberOfPages
    {
      get { return (int)GetValue(TotalNumberOfPagesProperty); }
      set { SetValue(TotalNumberOfPagesProperty, value); }
    }

    public SinglePrintedPage()
    {
      InitializeComponent();
    }
  }
}