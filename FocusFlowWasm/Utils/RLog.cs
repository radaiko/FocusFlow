// ***********************************************************************
// File              : RLog.cs
// Assembly          : Core
// Author            : aikoradlingmayr
// Created           : 08-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 12-10-2024
// ***********************************************************************

#region
using System.Runtime.CompilerServices;
#endregion

namespace FocusFlowWasm.Utils;

public static class RLog {
  private enum ELogType {
    Info
    , Warning
    , Error
    , Debug
  }

  private static bool _isDebugEnabled = false;
  private static bool _isFileLoggingEnabled = false;
  private static bool _isConsoleEnabled = true;
  private static int _longestFilename;
  private static int _longestMethodName;
  private static int _longestLineNumber;
  private static readonly object Lock = new();

  // TODO: add a file retention logic
  // TODO: extend to support multiple log files with different settings
  public static string? LogPath { get; private set; }
  
  public static void AddConsole() => _isConsoleEnabled = true;
  public static void RemoveConsole() => _isConsoleEnabled = false;
  public static void AddFile(string path) {
    LogPath = path;
    _isFileLoggingEnabled = true;
  }
  public static void RemoveFile() {
    LogPath = null;
    _isFileLoggingEnabled = false;
  }
  public static void AddDebug() => _isDebugEnabled = true;
  public static void RemoveDebug() => _isDebugEnabled = false;
  

  /// <summary>
  ///   Returns a string array with the last x lines from the log
  /// </summary>
  /// <param name="linesToRead"></param>
  /// <returns></returns>
  public static IEnumerable<string> Get(int linesToRead = 100) => LogPath != null ? File.ReadLines(LogPath).TakeLast(linesToRead) : [];

  /// <summary>
  ///   Return a string array with all log lines. Attention array could be big.
  /// </summary>
  /// <returns></returns>
  public static IEnumerable<string> GetAll() => LogPath != null ? File.ReadAllLines(LogPath) : [];


  /// <summary>
  ///   Write a new Info entry
  /// </summary>
  /// <param name="message"></param>
  /// <param name="caller"></param>
  /// <param name="sourceFilePath"></param>
  /// <param name="lineNumber"></param>
  public static void Info(string message, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0) {
    Write(message, caller, sourceFilePath, lineNumber);
  }

  /// <summary>
  ///   Write a new Warning entry
  /// </summary>
  /// <param name="message"></param>
  /// <param name="caller"></param>
  /// <param name="sourceFilePath"></param>
  /// <param name="lineNumber"></param>
  public static void Warning(string message, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0) {
    Write(message, caller, sourceFilePath, lineNumber, ELogType.Warning);
  }

  /// <summary>
  ///   Write a new Error entry
  /// </summary>
  /// <param name="message"></param>
  /// <param name="caller"></param>
  /// <param name="sourceFilePath"></param>
  /// <param name="lineNumber"></param>
  public static void Error(string message, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0) {
    Write(message, caller, sourceFilePath, lineNumber, ELogType.Error);
  }

  /// <summary>
  ///   Write a new Error entry
  /// </summary>
  /// <param name="message"></param>
  /// <param name="e"></param>
  /// <param name="caller"></param>
  /// <param name="sourceFilePath"></param>
  /// <param name="lineNumber"></param>
  public static void Error(string message, Exception e, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0) {
    var newMessage = $"{message} {e.Message} {e.StackTrace}";
    Write(newMessage, caller, sourceFilePath, lineNumber, ELogType.Error);
  }

  /// <summary>
  ///   Write a Debug entry if activated
  /// </summary>
  /// <param name="message"></param>
  /// <param name="caller"></param>
  /// <param name="sourceFilePath"></param>
  /// <param name="lineNumber"></param>
  public static void Debug(string message, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0) {
    if (_isDebugEnabled)
      Write(message, caller, sourceFilePath, lineNumber, ELogType.Debug);
  }

  private static void Write(string message, string callername, string callerPath, int lineNumber, ELogType type = ELogType.Info) {
    var filename = Path.GetFileName(callerPath);
    if (_longestFilename < filename.Length)
      _longestFilename = filename.Length;
    if (_longestMethodName < callername.Length)
      _longestMethodName = callername.Length;
    if (_longestLineNumber < lineNumber.ToString().Length)
      _longestLineNumber = lineNumber.ToString().Length;
    var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

    var line = $"{timestamp} {type.ToString().PadLeft(7).ToUpper()} [{filename.PadLeft(_longestFilename)}:{lineNumber.ToString().PadLeft(_longestLineNumber)}]: {message}";


    if (_isConsoleEnabled) {
      Console.WriteLine(line);
    }

    if (_isFileLoggingEnabled) {
      lock (Lock) {
        using var fs = new FileStream(LogPath, FileMode.Append, FileAccess.Write);
        using var sw = new StreamWriter(fs);
        sw.WriteLine(line);
      }
    }
  }
}