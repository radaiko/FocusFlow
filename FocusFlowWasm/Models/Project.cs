// ***********************************************************************
// File              : Project.cs
// Assembly          : Core
// Author            : aikoradlingmayr
// Created           : 15-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 15-10-2024
// ***********************************************************************
namespace FocusFlowWasm.Models;

// Projects
  // Human Description
    // every folder is a project
    // sub folders can be sub projects splitted by "/"
    // every project have an inbox for quickly adding nodes and the all open todos overview
  // Code Description
    // Name
    // Children (nodes or parents)
    // CountOfChildren

public class Project : Element {
  // TODO: implement the inbox logic
  
  #region Properties -----------------------------------------------------------
  public string? Name { get; set; }
  public List<Element> Children { get; } = [];
  public int CountOfChildren => Children.Count;
  #endregion
  
  #region Static Methods -------------------------------------------------------
  public static Project Load(string path) {
    throw new NotImplementedException();
    // TODO: implement the logic to load all projects from the given path
  }
  #endregion
}