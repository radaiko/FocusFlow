// ***********************************************************************
// File              : Logger.cs
// Assembly          : Core
// Author            : aikoradlingmayr
// Created           : 14-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 14-10-2024
// ***********************************************************************
using System.Runtime.CompilerServices;

namespace FocusFlowWeb.Utils;

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
  private static int _longestFilename;
  private static int _longestMethodName;
  private static int _longestLineNumber;
  private static bool _writeToConsole;
  private static bool _writeToFile;
  private static bool _isDebugEnabled;
  private static readonly object Lock = new();

  public static void AddConsole() {
    _writeToConsole = true;
  }

  public static void AddFile(string path) {
    _writeToFile = true;
    LogPath = path;
  }

  public static void AddDebug() {
    _isDebugEnabled = true;
  }
  
  // TODO: add a file retention logic
  public  static string? LogPath { get; private set; }

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
  ///   Log a new Info entry
  /// </summary>
  /// <param name="message"></param>
  /// <param name="caller"></param>
  /// <param name="sourceFilePath"></param>
  /// <param name="lineNumber"></param>
  public static void Info(string message, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0) {
    Log(message, caller, sourceFilePath, lineNumber);
  }

  /// <summary>
  ///   Log a new Warning entry
  /// </summary>
  /// <param name="message"></param>
  /// <param name="caller"></param>
  /// <param name="sourceFilePath"></param>
  /// <param name="lineNumber"></param>
  public static void Warning(string message, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0) {
    Log(message, caller, sourceFilePath, lineNumber, ELogType.Warning);
  }

  /// <summary>
  ///   Log a new Error entry
  /// </summary>
  /// <param name="message"></param>
  /// <param name="caller"></param>
  /// <param name="sourceFilePath"></param>
  /// <param name="lineNumber"></param>
  public static void Error(string message, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0) {
    Log(message, caller, sourceFilePath, lineNumber, ELogType.Error);
  }

  /// <summary>
  ///   Log a new Error entry
  /// </summary>
  /// <param name="message"></param>
  /// <param name="e"></param>
  /// <param name="caller"></param>
  /// <param name="sourceFilePath"></param>
  /// <param name="lineNumber"></param>
  public static void Error(string message, Exception e, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0) {
    var newMessage = $"{message} {e.Message} {e.StackTrace}";
    Log(newMessage, caller, sourceFilePath, lineNumber, ELogType.Error);
  }

  /// <summary>
  ///   Log a Debug entry if activated
  /// </summary>
  /// <param name="message"></param>
  /// <param name="caller"></param>
  /// <param name="sourceFilePath"></param>
  /// <param name="lineNumber"></param>
  public static void Debug(string message, [CallerMemberName] string caller = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int lineNumber = 0) {
    if (_isDebugEnabled)
      Log(message, caller, sourceFilePath, lineNumber, ELogType.Debug);
  }
  
  private static void Log(string message, string callername, string callerPath, int lineNumber, ELogType type = ELogType.Info) {
    var filename = Path.GetFileName(callerPath);
    if (_longestFilename < filename.Length)
      _longestFilename = filename.Length;
    if (_longestMethodName < callername.Length)
      _longestMethodName = callername.Length;
    if (_longestLineNumber < lineNumber.ToString().Length)
      _longestLineNumber = lineNumber.ToString().Length;
    var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");

    // prepare line
    var line = $"{timestamp} {type.ToString().PadLeft(7).ToUpper()} [{filename.PadLeft(_longestFilename)}:{lineNumber.ToString().PadLeft(_longestLineNumber)}]: {message}";

    if (_writeToConsole) {
      Console.WriteLine(line);
    }
    if (_writeToFile) {
      if (LogPath == null) {
        throw new Exception("LogPath is not set");
      }
      lock (Lock) {
        using var fs = new FileStream(LogPath, FileMode.Append, FileAccess.Write);
        using var sw = new StreamWriter(fs);
        sw.WriteLine(line);
      }
    }
  }
}