using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FamilyArchive.Domain.Enums;

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
    

    public void RemoveName(Guid memberNameId)
    {
        _names.RemoveAll(_names => _names.Id == memberNameId);
    }

    public void AddChild(Member child, string relationshipType, DateTime? establishedDate = null)
    {
        var relationship = new MemberRelationship
        {
            Id = Guid.NewGuid(),
            Member = child,
            MemberId = child.Id,
            RelatedMember = this,
            RelatedMemberId = this.Id,
            RelationshipType = relationshipType,
            EstablishedDate = establishedDate
        };
        child.Relationships.Add(relationship);
        this.ParentRelationships.Add(relationship);
    }

    public void UpdateName(Guid memberNameId, string value, NameType? type, string? otherNameType, int order, bool hidden)
    {
        var name = _names.FirstOrDefault(n => n.Id == memberNameId);
        if (name == null)
            throw new InvalidOperationException("Name not found.");

        if (_names.Any(n => n.Order == order && n.Id != memberNameId))
            throw new InvalidOperationException("Cannot set a duplicate order.");

        name.Value = value;
        name.Type = type;
        name.OtherNameType = otherNameType;
        name.Order = order;
        name.Hidden = hidden;
    }
    public void ReorderName(Guid memberNameId, int newOrder)
    {
        //add logic to shift names dynamically
        var name = _names.FirstOrDefault(n => n.Id == memberNameId);
        if (name == null)
            throw new InvalidOperationException("Name not found.");
        if (_names.Any(n => n.Order == newOrder && n.Id != memberNameId))
            throw new InvalidOperationException("Cannot set a duplicate order.");
        name.Order = newOrder;
    }
}