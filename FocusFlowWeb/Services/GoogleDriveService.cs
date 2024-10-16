// ***********************************************************************
// File              : GoogleDriveService.cs
// Assembly          : FocusFlowWeb
// Author            : aikoradlingmayr
// Created           : 15-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 15-10-2024
// ***********************************************************************

using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using File = Google.Apis.Drive.v3.Data.File;

namespace FocusFlowWeb.Services
{
  public class GoogleDriveService
  {
    private readonly DriveService _driveService;

    public GoogleDriveService(DriveService driveService)
    {
      _driveService = driveService;
    }

    public async Task<IList<File>> ListFilesAsync()
    {
      var request = _driveService.Files.List();
      request.Fields = "nextPageToken, files(id, name)";
      var result = await request.ExecuteAsync();
      return result.Files;
    }
  }
}