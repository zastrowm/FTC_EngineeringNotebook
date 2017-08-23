//using System;
//using System.Collections.Generic;
//using System.Linq;
//using EngineeringNotebook.Model;
//using PropertyChanged;

//namespace EngineeringNotebook.Mvvm
//{
//  [ImplementPropertyChanged]
//  public class ImagesCollectionViewModel : BaseCollectionViewModel<ImageViewModel>
//  {
//    public void LoadModel(ImageModel[] models)
//    {
//      if (models == null || models.Length == 0)
//        return;

//      Items.Clear();

//      foreach (var imageModel in models)
//      {
//        var imageVm = new ImageViewModel()
//                      {
//                        Caption = imageModel.Caption,
//                        Description = imageModel.Description,
//                      };

//        if (!string.IsNullOrWhiteSpace(imageModel.Base64ImageData))
//        {
//          imageVm.Image = new ImageData(0, 0, Convert.FromBase64String(imageModel.Base64ImageData));
//        }

//        Items.Add(imageVm);
//      }
//    }

//    public ImageModel[] SaveModel()
//    {
//      return Items.Where(i => i.IsNonEmpty)
//                  .Select(i => new ImageModel()
//                               {
//                                 Base64ImageData = i.Image.Data != null ? Convert.ToBase64String(i.Image.Data) : null,
//                                 Caption = i.Caption,
//                                 Description = i.Description
//                               })
//                  .ToArray();
//    }
//  }

//  [ImplementPropertyChanged]
//  public class ImageViewModel
//  {
//    public ImageData Image { get; set; }
//    public string Caption { get; set; }
//    public string Description { get; set; }

//    public bool IsNonEmpty
//      => Image.Data != null || !string.IsNullOrWhiteSpace(Caption) || !string.IsNullOrWhiteSpace(Description);
//  }
//}

using System;
using System.Collections.Generic;
using System.Linq;