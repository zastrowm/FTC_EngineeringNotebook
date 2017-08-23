using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using NotebookApp;
using PropertyChanged;

namespace EngineeringNotebook.Mvvm
{
  /// <summary> The people present for the activity. </summary>
  [ImplementPropertyChanged]
  public class AttendanceViewModel : BaseViewModel
  {
    public AttendanceViewModel()
    {
      AvailableIndividuals.ListChanged += delegate { OnPropertyChanged(nameof(AvailableIndividuals)); };
    }

    public Individual Author { get; set; }

    public BindingList<Individual> AvailableIndividuals { get; }
      = new BindingList<Individual>(
        AppConfigurationSettings.Instance.Members
                                .Select(s => new Individual(s))
                                .ToList()
      );

    public void SetAuthorAndAttendees(string author, string[] attended)
    {
      Author = Find(author);

      if (attended == null)
        return;

      foreach (var attendee in attended)
      {
        var himOrHer = Find(attendee);
        if (himOrHer != null)
        {
          himOrHer.DidAttend = true;
        }
      }
    }

    private Individual Find(string name)
    {
      return AvailableIndividuals.FirstOrDefault(a => a.DisplayName == name);
    }

    public string[] GetModel()
    {
      return AvailableIndividuals
        .Where(i => i.DidAttend)
        .Select(i => i.DisplayName)
        .ToArray();
    }
  }

  [ImplementPropertyChanged]
  public class Individual
  {
    /// <summary> Constructor. </summary>
    /// <param name="displayName"> Name of the display. </param>
    public Individual(string displayName)
    {
      DisplayName = displayName;
    }

    public Individual()
    {
      DisplayName = null;
    }

    public bool DidAttend { get; set; }

    public string DisplayName { get; protected set; }
  }
}