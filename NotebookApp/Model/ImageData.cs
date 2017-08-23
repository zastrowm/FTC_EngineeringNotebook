using System;
using System.Collections.Generic;
using System.Linq;

namespace EngineeringNotebook.Mvvm
{
  /// <summary> Represents an image loaded from the file system. </summary>
  public struct ImageData
  {
    public ImageData(int width, int height, byte[] data)
    {
      Width = width;
      Height = height;
      Data = data;
    }

    /// <summary> The width of the image. </summary>
    public int Width { get; }

    /// <summary> The height of the image. </summary>
    public int Height { get; }

    /// <summary> The data for the image. </summary>
    public byte[] Data { get; }

    public bool IsEmpty
      => Data == null;
  }
}