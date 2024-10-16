// ***********************************************************************
// File              : Settings.cs
// Assembly          : Core
// Author            : aikoradlingmayr
// Created           : 14-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 14-10-2024
// ***********************************************************************
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;

// Settings
// Human Description
// a class where settings are saved and loaded
// Code Description
// Base Folder (iCloud and Google Drive support on mobile)
// List<CalendarICS> CalendarIcs  // list of calendar.ics which we should import - refresh on every on focus or 10 minutes
public class Settings : INotifyPropertyChanged {
  private string? _storageDirectory;
  private List<CalendarIcs> _calendarIcs = new();

  public string? StorageDirectory {
    get => _storageDirectory;
    set {
      if (_storageDirectory != value) {
        _storageDirectory = value;
        OnPropertyChanged();
      }
    }
  }

  public List<CalendarIcs> CalendarIcs {
    get => _calendarIcs;
    set {
      if (_calendarIcs != value) {
        _calendarIcs = value;
        OnPropertyChanged();
      }
    }
  }

  public event PropertyChangedEventHandler? PropertyChanged;

  protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    SaveSettings();
  }

  private void SaveSettings() {
    DataHandler.SaveSettings(this);
  }
}