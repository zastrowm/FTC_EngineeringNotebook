using System;
using System.Collections.Generic;
using System.Linq;
using PropertyChanged;

namespace EngineeringNotebook.Mvvm
{
  /// <summary> Holds a single image and an associated caption. </summary>
  [ImplementPropertyChanged]
  public class ImageAndCaption : BaseViewModel
  {
    /// <summary> The image association with this instance. </summary>
    public ImageData Image { get; set; }

    /// <summary> The caption associated with this instance. </summary>
    public string Caption { get; set; }
  }
}