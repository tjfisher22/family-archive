using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyArchive.Domain.Entities;

/// <summary>
/// Represents a family group, specifically listing all children of a set of given parents.
/// </summary>
public class Family
{
    /// <summary>
    /// Unique identifier for the family.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the family (e.g., "Smith Family").
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Optional description of the family.
    /// </summary>
    public string? Description { get; set; }

    //public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    //public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The members belonging to this family.
    /// </summary>
    public ICollection<Member> Members { get; set; } = new List<Member>();
}
