// ***********************************************************************
// File              : DataHandler.cs
// Assembly          : Core
// Author            : aikoradlingmayr
// Created           : 14-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 14-10-2024
// ***********************************************************************
using System.Text.Json;
using FocusFlowWeb.Models;
using FocusFlowWeb.Services;

namespace FocusFlowWeb.Utils;

public static class DataHandler {
  public static void SaveSettings(Settings settings) {
    var json = JsonSerializer.Serialize(settings);
    var path = Path.Combine(GetConfigFolder(), "Preferences", "settings.json");
    WriteToDisk(path, json);
  }

  public static Settings LoadSettings() {
    var path = Preferences.Get("LastSettingsPath", null);
    if (path == null) {
      Logger.Warning("Last settings path not found. Return empty settings");
      return new Settings();
    }
    try {
      var json = File.ReadAllText(path);
      var settings = JsonSerializer.Deserialize<Settings>(json);
      if (settings != null) return settings;
      Logger.Error("Error while deserializing settings. Return new settings instead");
      return new Settings();
    }
    catch (Exception ex) {
      Logger.Error("Error while loading settings. Return new settings instead", ex);
      return new Settings();
    }
  }

  public static string GetLogPath() => Path.Combine(GetConfigFolder(), "Logs", "log.txt");

  private static async void WriteToDisk(string path, string content, bool createDirectory = true) {
    if (createDirectory && !Directory.Exists(Path.GetDirectoryName(path))) {
      Logger.Warning($"Directory {Path.GetDirectoryName(path)} does not exist. Create it");
      Directory.CreateDirectory(Path.GetDirectoryName(path)!);
    }
    await File.WriteAllTextAsync(path, content);
  }

  private static string? GetStorageFolder() {
    return SettingsService.It.Settings.StorageDirectory;
  }
  
  

  private static string? GetConfigFolder() {
    string libraryPath = string.Empty;
    
    return libraryPath;
  }
}