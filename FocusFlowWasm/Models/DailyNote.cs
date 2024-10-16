// ***********************************************************************
// File              : DailyNote.cs
// Assembly          : UI
// Author            : aikoradlingmayr
// Created           : 15-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 15-10-2024
// ***********************************************************************
namespace FocusFlowWasm.Models;

// DailyNote
  // Human Description
    // a list of nodes with predefined content from a selectable content
    // DailyNote will be automatically archived into the project where it was created on the next day
    // Auto adding meetings from outlook com or CalDav
  // Code Description
    // List<Node> Notes
    // DateTime Day
    // List<Node> Meetings

public class DailyNote : Element {
  public List<Node> Notes { get; } = [];
  public DateTime Day { get; set; }
  public List<Node> Meetings { get; } = [];
}