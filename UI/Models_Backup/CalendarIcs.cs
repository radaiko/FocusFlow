// ***********************************************************************
// File              : CalendarIcs.cs
// Assembly          : Core
// Author            : aikoradlingmayr
// Created           : 15-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 15-10-2024
// ***********************************************************************
namespace FocusFlowWeb.Models;

// CalendarIcs
  // Human Description
    // a class which represents a calendar ics file
  // Code Description
    // Link
    // Name
    // Active
    // LastRefresh
    // NextRefresh

public class CalendarIcs {
  public string? Link { get; set; }
  public string? Name { get; set; }
  public bool Active { get; set; } = false;
  public DateTime LastRefresh { get; set; }
  public DateTime NextRefresh { get; set; }
}