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
    public Guid? ClanId { get; set; }
    public Clan? Clan { get; set; }

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
        if (child.Id == this.Id)
            throw new InvalidOperationException("A member cannot be their own child.");

        // Prevent circular parent-child relationships
        if (child.ParentRelationships.Any(r => r.MemberId == this.Id))
            throw new InvalidOperationException("Circular parent-child relationship detected.");

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
    public void RemoveChild(Member child)
    {
        var relationship = child.Relationships.FirstOrDefault(r => r.RelatedMemberId == this.Id);
        if (relationship != null)
        {
            child.Relationships.Remove(relationship);
            this.ParentRelationships.Remove(relationship);
        }
    }

    public void UpdateNameValue(Guid memberNameId, string value)
    {
        var name = _names.FirstOrDefault(n => n.Id == memberNameId);
        if (name == null)
            throw new InvalidOperationException("Name not found.");

        name.Value = value;
    }
    public void UpdateNameType(Guid memberNameId, NameType? type, string? otherNameType)
    {
        var name = _names.FirstOrDefault(n => n.Id == memberNameId);
        if (name == null)
            throw new InvalidOperationException("Name not found.");

        name.Type = type;
        name.OtherNameType = otherNameType;
    }
    public void UpdateNameOrder(Guid memberNameId, int order)
    {
        var name = _names.FirstOrDefault(n => n.Id == memberNameId);
        if (name == null)
            throw new InvalidOperationException("Name not found.");

        if (_names.Any(n => n.Order == order && n.Id != memberNameId))
            throw new InvalidOperationException("Cannot set a duplicate order.");

        name.Order = order;
    }
    public void UpdateNameHidden(Guid memberNameId, bool hidden)
    {
        var name = _names.FirstOrDefault(n => n.Id == memberNameId);
        if (name == null)
            throw new InvalidOperationException("Name not found.");

        name.Hidden = hidden;
    }
    public void ReorderName(Guid memberNameId, int newOrder)
    {
        //add logic to shift names dynamically
        //enforce unique order starting at 0
    }
}