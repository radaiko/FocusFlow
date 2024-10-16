// ***********************************************************************
// File              : Node.cs
// Assembly          : Core
// Author            : aikoradlingmayr
// Created           : 15-10-2024
// 
// Last Modified By   : aikoradlingmayr
// Last Modified On   : 15-10-2024
// ***********************************************************************
namespace FocusFlowWeb.Models;

// Node
  // Human Description
    // A node represent a line, the line itself could have more lines in the editor few but in the background it's a single content string
    // A node is always shown with a dash to the right and intended by the parent
    // The content of a node could be text, links, images or whole code block
    // if the content is an image or a code block show only a summary / alt text on only show the full content when open it
  // Code Description
    // Parent (Node or Project)
    // Children (Node or Project)
    // Content
    // Created
    // Updated 
public class Node : Element {
  public Element? Parent { get; set; }
  public List<Element> Children { get; } = [];
  public string? Content { get; set; }
  public DateTime? Created { get; set; }
  public DateTime? Updated { get; set; }
  
}