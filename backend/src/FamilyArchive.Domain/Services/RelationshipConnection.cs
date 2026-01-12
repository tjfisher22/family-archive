using FamilyArchive.Domain.Entities;

namespace FamilyArchive.Domain.Services;

/// <summary>
/// Represents the bidirectional connection between two family members through a common ancestor.
/// </summary>
public class RelationshipConnection
{
    /// <summary>
    /// Path from the first member up to the common ancestor
    /// </summary>
    public RelationshipPath PathFromMemberA { get; set; } = default!;
    
    /// <summary>
    /// Path from the second member up to the common ancestor
    /// </summary>
    public RelationshipPath PathFromMemberB { get; set; } = default!;
    
    /// <summary>
    /// The lowest common ancestor shared by both members
    /// </summary>
    public Member CommonAncestor { get; set; } = default!;
    
    /// <summary>
    /// Number of generations from member A to the common ancestor
    /// </summary>
    public int GenerationsUpFromA => PathFromMemberA.Steps.Count;
    
    /// <summary>
    /// Number of generations from member B to the common ancestor
    /// </summary>
    public int GenerationsUpFromB => PathFromMemberB.Steps.Count;
}