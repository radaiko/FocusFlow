// ***********************************************************************
// File              : Logger.cs
// Assembly          : Core
// Author            : aikoradlingmayr
// Created           : 14-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 14-10-2024
// ***********************************************************************
namespace Core;

#region
using System.Runtime.CompilerServices;
#endregion

/// <summary>
///   A simple local file logger
/// </summary>
public static class Logger {
  private enum ELogType {
    Info
    , Warning
    , Error
    , Debug
  }

  /// <summary>
  ///   Set to true if you want to enable debug logging
  /// </summary>
  public static bool IsDebugEnabled;
  public static bool WriteToConsoleAsWell;
  private static int _longestFilename;
  private static int _longestMethodName;
  private static int _longestLineNumber;
  private static readonly object Lock = new();

  // TODO: add a file retention logic
  public static string? LogPath { get; set; }

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
    if (IsDebugEnabled)
      Write(message, caller, sourceFilePath, lineNumber, ELogType.Debug);
  }

  public static void SetDefault() {
    WriteToConsoleAsWell = true;
    IsDebugEnabled = false;
  }

  private static void Write(string message, string callername, string callerPath, int lineNumber, ELogType type = ELogType.Info) {
    if (LogPath == null) return;
    var filename = Path.GetFileName(callerPath);
    if (_longestFilename < filename.Length)
      _longestFilename = filename.Length;
    if (_longestMethodName < callername.Length)
      _longestMethodName = callername.Length;
    if (_longestLineNumber < lineNumber.ToString().Length)
      _longestLineNumber = lineNumber.ToString().Length;
    var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

    lock (Lock) {
      using var fs = new FileStream(LogPath, FileMode.Append, FileAccess.Write);
      using var sw = new StreamWriter(fs);

      // if testing
      // if (Misc.IsTestRun) {
      //   timestamp = "2024-05-05 12:00:00";
      //   lineNumber = 0;
      //   _longestLineNumber = 0;
      // }

      var line = $"{timestamp} {type.ToString().PadLeft(7).ToUpper()} [{filename.PadLeft(_longestFilename)}:{lineNumber.ToString().PadLeft(_longestLineNumber)}]: {message}";

      sw.WriteLine(line);
      if (WriteToConsoleAsWell)
        Console.WriteLine(line);
    }
  }
}