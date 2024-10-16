// ***********************************************************************
// File              : FolderPickerService.cs
// Assembly          : UI
// Author            : aikoradlingmayr
// Created           : 15-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 15-10-2024
// ***********************************************************************
using CommunityToolkit.Maui.Storage;

namespace UI.Services;

public interface IFolderPickerService
{
  Task<string> PickFolderAsync();
}

public class FolderPickerService : IFolderPickerService
{
  public async Task<string> PickFolderAsync()
  {
    var result = await FolderPicker.Default.PickAsync();
    return ((bool)result?.IsSuccessful ? result.Folder?.Path : string.Empty) ?? string.Empty;
  }
}