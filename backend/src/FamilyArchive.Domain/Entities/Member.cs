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
    public IReadOnlyCollection<MemberName> Names => _names.AsReadOnly();
        private List<MemberName> _names = new();
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public string? Gender { get; set; }

    // Relationships - Dynamic to allow many different types of relationships (only vertical relationships and partners are modeled)

    public ICollection<MemberRelationship> Relationships { get; set; } = new List<MemberRelationship>(); // as child
    public ICollection<MemberRelationship> ParentRelationships { get; set; } = new List<MemberRelationship>(); // as parent
    public ICollection<MemberPartnership> Partnerships { get; set; } = new List<MemberPartnership>(); // as member
    public ICollection<MemberPartnership> PartnerPartnerships { get; set; } = new List<MemberPartnership>(); // as partner

    // For grouping into families/clans
    public Guid? FamilyId { get; set; }
    public Family? Family { get; set; }

    public void AddName(MemberName name)
    {
        if (_names.Any(n => n.Order == name.Order))
            throw new InvalidOperationException("Cannot add a name with a duplicate order.");
        _names.Add(name);
    }
}