// ***********************************************************************
// File              : Meeting.cs
// Assembly          : UI
// Author            : aikoradlingmayr
// Created           : 15-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 15-10-2024
// ***********************************************************************
namespace FocusFlowWasm.Models;

// Meeting
  // Human Description
    // a node which represents a meeting
  // Code Description
    // List<Node> Childrens
    // DateTime StartTime
    // DateTime EndTime
    // string? Title

public class Meeting : Node {
  public List<Node> Childrens { get; } = [];
  public DateTime? StartTime { get; set; }
  public DateTime? EndTime { get; set; }
  public string? Title { get; set; }
}