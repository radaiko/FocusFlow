// ***********************************************************************
// File              : GoogleDriveService.cs
// Assembly          : FocusFlowWeb_Backup
// Author            : aikoradlingmayr
// Created           : 15-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 15-10-2024
// ***********************************************************************
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace FocusFlowWeb.Services;

public class GoogleDriveService {
  private static readonly string[] Scopes = { DriveService.Scope.DriveFile };
  private const string ApplicationName = "FocusFlowWeb_Backup";
  private readonly DriveService _driveService;

  public GoogleDriveService() {
    var credential = GetCredentials();
    _driveService = new DriveService(new BaseClientService.Initializer {
      HttpClientInitializer = credential,
      ApplicationName = ApplicationName,
    });
  }

  private UserCredential GetCredentials() {
    using var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read);
    string credPath = "token.json";
    return GoogleWebAuthorizationBroker.AuthorizeAsync(
      GoogleClientSecrets.Load(stream).Secrets,
      Scopes,
      "user",
      CancellationToken.None,
      new FileDataStore(credPath, true)).Result;
  }

  public string UploadFile(string filePath, string mimeType) {
    var fileMetadata = new Google.Apis.Drive.v3.Data.File {
      Name = Path.GetFileName(filePath)
    };
    FilesResource.CreateMediaUpload request;
    using (var stream = new FileStream(filePath, FileMode.Open)) {
      request = _driveService.Files.Create(fileMetadata, stream, mimeType);
      request.Fields = "id";
      request.Upload();
    }
    var file = request.ResponseBody;
    return file.Id;
  }

  public void DownloadFile(string fileId, string saveToPath) {
    var request = _driveService.Files.Get(fileId);
    using var stream = new MemoryStream();
    request.Download(stream);
    using var fileStream = new FileStream(saveToPath, FileMode.Create, FileAccess.Write);
    stream.WriteTo(fileStream);
  }
}