// ***********************************************************************
// File              : DataHandler.cs
// Assembly          : Core
// Author            : aikoradlingmayr
// Created           : 14-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 14-10-2024
// ***********************************************************************
namespace Core;

public static class DataHandler {
  
  public static Settings GetSettings() {
    // TODO: Load settings from disk
    return new Settings {
      StorageDirectory = "/data/"
    };
  }
  
  public static void SaveSettings(Settings settings) {
    // TODO: Save settings to disk
  }
}