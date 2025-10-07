using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FamilyArchive.Domain.Entities;

public class Member
{
    public Guid Id { get; set; }
    // List of names (first, middle, last, maiden, chosen, nickname, etc.)
    public ICollection<MemberName> Names { get; set; } = new List<MemberName>();
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public string? Gender { get; set; }

    // Rlationships - Dynamic to allow many different types of relationships (only vertical relationships are modeled)
    public ICollection<MemberRelationship> Relationships { get; set; } = new List<MemberRelationship>();
    public ICollection<MemberPartnership> Partnerships { get; set; } = new List<MemberPartnership>();

    // For grouping into families/clans
    public Guid? FamilyId { get; set; }
    public Family? Family { get; set; }
}