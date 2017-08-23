using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using EngineeringNotebook.Mvvm;
using EngineeringNotebook.Views;

namespace EngineeringNotebook
{
  /// <summary>
  /// Interaction logic for EntryPageDialog.xaml
  /// </summary>
  public partial class EntryPageControl
  {
    public EntryPageControl()
    {
      InitializeComponent();
    }

    public PageEntryViewModel ViewModel
      => (PageEntryViewModel)DataContext;

    private void HandleEditAttendance(object sender, RoutedEventArgs e)
    {
      var window = new Window
                   {
                     Content = new AttendanceControl(),
                     DataContext = ViewModel,
                     Owner = Window.GetWindow(this),
                     Title = "Edit Entry Information",
                     SizeToContent = SizeToContent.WidthAndHeight,
                     Style = (Style)FindResource("PrimaryWindow"),
                   };

      window.ShowDialog();
    }

    private void CloseDropDown(object sender, RoutedEventArgs e)
    {
      AddDropDown.IsOpen = false;
    }

    private void RemoveSection(object sender, RoutedEventArgs e)
    {
      var dataContext = (sender as FrameworkElement)?.DataContext
                        ?? (sender as Hyperlink)?.DataContext;

      var section = (SubSectionEntryViewModel)dataContext;
      section.Owner.RemoveSectionCommand.Execute(section);
    }

    private void RemoveImage(object sender, RoutedEventArgs e)
    {
    }
  }
}