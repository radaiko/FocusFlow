// ***********************************************************************
// File              : ToDo.cs
// Assembly          : UI
// Author            : aikoradlingmayr
// Created           : 15-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 15-10-2024
// ***********************************************************************
namespace FocusFlowWasm.Models;

// Todo
  // Human Description
    // A todo will be detected by the editor when "TODO:" or /TODO is written
    // A small todo popup will be shown where the user can input the properties
    // Alternative a inline todo creator could be used (set able in the settings)
    // indicated by "TODO:"
    // followed by the project, projects should be suggested while typing
    // esc cancel the suggestion until " "
    // enter insert the project
    // followed by priority, due date and estimated time
  // Code Description
    // short name
    // description
    // project
    // priority
    // due date
    // estimated time

public class ToDo : Element {
  public string? Name { get; set; }
  public string? Description { get; set; }
  public Project? Project { get; set; }
  public EToDoPriority Priority { get; set; } = EToDoPriority.Medium;
  public DateTime? DueDate { get; set; }
  public TimeSpan? EstimatedTime { get; set; }
  public EToDoStatus Status { get; set; } = EToDoStatus.Open;

}

public enum EToDoStatus {
  Open,
  InProgress,
  Done
}

public enum EToDoPriority {
  Immediate = 1,
  High = 2,
  Medium = 3,
  Low = 4
}