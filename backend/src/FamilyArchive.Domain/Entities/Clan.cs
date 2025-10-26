using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyArchive.Domain.Entities;
/// <summary>
/// Represents a clan, which is a group of related family members, often spanning multiple generations.
/// </summary>
public class Clan
{
    /// <summary>
    /// Unique identifier for the clan.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the clan.
    /// </summary>
    public string Name { get; set; } = default!;

    /// <summary>
    /// Optional description of the clan.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The members belonging to this clan.
    /// </summary>
    public ICollection<Member> Members { get; set; } = new List<Member>();
}
