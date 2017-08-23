using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
using EngineeringNotebook.Mvvm;
using NotebookApp.Printing;
using NotebookApp.Properties;

namespace NotebookApp
{
  /// <summary> Creates pages so that a viewmodel can be printed. </summary>
  public class PageCreator
  {
    private const int PixelsPerInch = 96;

    /// <summary> The viewmodel that will be printed. </summary>
    private readonly PageEntryViewModel _viewModel;

    private readonly List<ContentPresenter> _controlsToPlace;
    private readonly string _practiceNumberTitle;

    /// <summary> Constructor. </summary>
    /// <param name="viewModel"> The viewmodel that will be printed. </param>
    /// <param name="practiceName"></param>
    public PageCreator(PageEntryViewModel viewModel, string practiceName)
    {
      _practiceNumberTitle = practiceName;
      _viewModel = viewModel;
      _controlsToPlace = CreateControlsThatNeedToBeLaidOut().ToList();
    }

    /// <summary> Saves the created xps document to a file. </summary>
    /// <param name="filename"> The name of the file to save the XPS document as. </param>
    public void SaveToFile(string filename)
    {
      var fixedDocument = new FixedDocument();

      foreach (var page in CreatePages())
      {
        fixedDocument.Pages.Add(page);
      }

      using (var memoryStream = new MemoryStream())
      using (Package container = Package.Open(filename, FileMode.Create))
      {
        using (XpsDocument xpsDoc = new XpsDocument(container, CompressionOption.Maximum))
        {
          XpsSerializationManager rsm = new XpsSerializationManager(new XpsPackagingPolicy(xpsDoc), false);
          rsm.SaveAsXaml(fixedDocument);
        }
        memoryStream.Position = 0;
      }
    }

    /// <summary>
    ///  Creates all a collection of pages that need to be printed if the view model were to be
    ///  printed.
    /// </summary>
    public IEnumerable<PageContent> CreatePages()
    {
      int pageNumber = 1;
      int startIndexToLayout = 0;

      var pagesToPrint = new List<SinglePrintedPage>();

      while (startIndexToLayout < _controlsToPlace.Count)
      {
        var currentPage = CreateNthPage(pageNumber);
        startIndexToLayout = LayoutAsManyControlsAsPossible(currentPage, startIndexToLayout);

        pagesToPrint.Add(currentPage);
        pageNumber++;
      }

      // we always want an even number of pages
      while (pageNumber % 2 != 1)
      {
        pagesToPrint.Add(CreateNthPage(pageNumber));
        pageNumber++;
      }

      // update the page counts to make sure that it actually works
      for (var i = 0; i < pagesToPrint.Count; i++)
      {
        SinglePrintedPage tempQualifier = pagesToPrint[i];
        tempQualifier.PageNumber = i + 1;
        tempQualifier.TotalNumberOfPages = pagesToPrint.Count;
      }

      return pagesToPrint.Select(ConstructPageContentFor);
    }

    /// <summary> Creates the SinglePrintedPage with the correct layout. </summary>
    private SinglePrintedPage CreateNthPage(int pageNumber)
    {
      bool shouldShowBorder = pageNumber % 2 == 0;

      Visibility showBorder = shouldShowBorder
        ? Visibility.Visible
        : Visibility.Collapsed;

      var margin = new Thickness(
                     Settings.Default.Margin_Left * PixelsPerInch,
                     Settings.Default.Margin_Top * PixelsPerInch,
                     Settings.Default.Margin_Right * PixelsPerInch,
                     Settings.Default.Margin_Bottom * PixelsPerInch
                   );

      var page = new SinglePrintedPage
                 {
                   Header = { Tag = _practiceNumberTitle },
                   Categories = { Visibility = showBorder },
                   Footer = { Visibility = showBorder },
                   Width = PixelsPerInch * 8.5 - margin.Left - margin.Right,
                   Height = PixelsPerInch * 11 - margin.Top - margin.Top,
                   DataContext = _viewModel,
                   Margin = margin
                 };

      // clear its children that are there only for mocking purposes. 
      page.PrimaryContent.Children.Clear();

      return page;
    }

    /// <summary />
    private IEnumerable<ContentPresenter> CreateControlsThatNeedToBeLaidOut()
    {
      yield return CreateControlUsingResource(_viewModel, "Print_ProcessSteps");
      yield return CreateControlUsingResource(_viewModel, "Print_DocumentInfo");
      yield return CreateControlUsingResource(_viewModel, "Print_Accomplishments");
      yield return CreateControlUsingResource(_viewModel, "Print_NextTasks");

      int imageAndBlurbCount = 0;

      foreach (var sectionItem in _viewModel.Sections.Items)
      {
        SubSectionType type = sectionItem.SectionType;
        var name = $"Print_{type}";

        // we want Image + blurbs to switch between left & right sides
        if (sectionItem.SectionType == SubSectionType.ImageWithCaptionAndBlurb)
        {
          imageAndBlurbCount++;
          if (imageAndBlurbCount % 2 == 0)
          {
            name += "_Right";
          }
        }

        yield return CreateControlUsingResource(sectionItem, name);
      }

      yield return CreateControlUsingResource(_viewModel, "Print_Reflections");
    }

    /// <summary> Put as many controls from the collection on the current page. </summary>
    /// <param name="currentPage"> The current page on which controls should be laid out. </param>
    /// <param name="controlIndex"> The index of the first control to add. </param>
    /// <returns> The index of the next control to lay out on the next page. </returns>
    private int LayoutAsManyControlsAsPossible(SinglePrintedPage currentPage,
                                               int controlIndex)
    {
      bool hasMoreSpace = true;

      while (controlIndex < _controlsToPlace.Count && hasMoreSpace)
      {
        var controlToPlace = _controlsToPlace[controlIndex];

        currentPage.PrimaryContent.Children.Add(controlToPlace);
        currentPage.UpdateLayout();

        currentPage.Measure(new Size(currentPage.Width, currentPage.Height));
        currentPage.Arrange(new Rect(0, 0, currentPage.Width, currentPage.Height));

        if (!IsUserVisible(controlToPlace, currentPage.AllowedSpace))
        {
          hasMoreSpace = false;
          currentPage.PrimaryContent.Children.Remove(controlToPlace);
          controlIndex--;

          if (controlIndex > 0)
          {
            // titles are always attached to the next sub-section
            var previousControl = _controlsToPlace[controlIndex];
            if ((previousControl.Content as SubSectionEntryViewModel)?.SectionType == SubSectionType.Title)
            {
              currentPage.PrimaryContent.Children.Remove(previousControl);
              controlIndex--;
            }
          }
        }

        controlIndex++;
      }
      return controlIndex;
    }

    /// <summary>
    ///  Creates a ContentPresent that shows the given content with the given static resource.
    /// </summary>
    private static ContentPresenter CreateControlUsingResource(object contentToShow,
                                                               string staticResourceName)
    {
      var controlForSection = new ContentPresenter()
                              {
                                Content = contentToShow,
                              };
      controlForSection.SetResourceReference(ContentPresenter.ContentTemplateProperty,
                                             staticResourceName);
      return controlForSection;
    }

    /// <summary> Create a new PageControl for the given printed page. </summary>
    private static PageContent ConstructPageContentFor(SinglePrintedPage singlePrintedPage)
    {
      var page = new PageContent();
      page.Child = new FixedPage();
      page.Child.Children.Add(singlePrintedPage);
      return page;
    }

    /// <summary> See if the given element is visible in the given container. </summary>
    private static bool IsUserVisible(FrameworkElement element, FrameworkElement container)
    {
      Rect bounds =
        element.TransformToAncestor(container)
               .TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
      Rect rect = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);
      return rect.Contains(bounds.TopLeft) && rect.Contains(bounds.BottomRight);
    }
  }
}