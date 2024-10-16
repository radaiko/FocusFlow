// ***********************************************************************
// File              : StaticLogger.cs
// Assembly          : UI
// Author            : aikoradlingmayr
// Created           : 15-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 15-10-2024
// ***********************************************************************
using Microsoft.Extensions.Logging;

namespace UI.Utils;

public static class StaticLogger<T> {
  
  public static ILogger GetStaticLogger => new Logger<T>(new LoggerFactory());

  
}