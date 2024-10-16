// ***********************************************************************
// File              : GoogleDriveService.cs
// Assembly          : FocusFlowWasm
// Author            : aikoradlingmayr
// Created           : 16-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 16-10-2024
// ***********************************************************************


using System.Text;
using FocusFlowWasm.Models;
using FocusFlowWasm.Utils;
using Microsoft.JSInterop;

namespace FocusFlowWasm.Services;

public class GoogleDriveService {
  private readonly IJSRuntime _jsRuntime;
  
  public bool IsInitialized { get; private set; }

  public GoogleDriveService(IJSRuntime jsRuntime) {
    _jsRuntime = jsRuntime;
  }

  public async void Init(string token) {
    RLog.Debug($"Setting access token: {token}");
    await _jsRuntime.InvokeVoidAsync("setAccessToken", token);
    IsInitialized = true;
  }
  
  public async Task<IEnumerable<GoogleDriveFile>> GetFiles() {
    if (!IsInitialized) throw new InvalidOperationException("GoogleDriveService not initialized");
    var files = await _jsRuntime.InvokeAsync<IEnumerable<GoogleDriveFile>>("getFiles");
    return files;
  }
  
  public async Task<GoogleDriveFile> GetFile(string id) {
    if (!IsInitialized) throw new InvalidOperationException("GoogleDriveService not initialized");
    var file = await _jsRuntime.InvokeAsync<GoogleDriveFile>("getFile", id);
    return file;
  }
  
  public async Task<string> GetFileContent(string id) {
    if (!IsInitialized) throw new InvalidOperationException("GoogleDriveService not initialized");
    var content = await _jsRuntime.InvokeAsync<string>("getFileContent", id);
    return content;
  }
  
  public async Task UploadFile(string fileName, string fileContent) {
    if (!IsInitialized) throw new InvalidOperationException("GoogleDriveService not initialized");
    using var stream = new MemoryStream(Encoding.UTF8.GetBytes(fileContent));
    using var reader = new StreamReader(stream);
    var content = await reader.ReadToEndAsync();
    await _jsRuntime.InvokeVoidAsync("uploadFile", fileName, content);
  }

  public async Task DeleteFile(string id) {
    if (!IsInitialized) throw new InvalidOperationException("GoogleDriveService not initialized");
    await _jsRuntime.InvokeVoidAsync("deleteFile", id);
  }
}