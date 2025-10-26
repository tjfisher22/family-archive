using FamilyArchive.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyArchive.Domain.Entities;

public class MemberRelationship
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; } // The member who has the relationship (typically the child)
    public Member Member { get; set; } = default!;
    public Guid RelatedMemberId { get; set; } // The member who is related to the above member (typically the parent)
    public Member RelatedMember { get; set; } = default!;
    public RelationshipType RelationshipType { get; set; } = default!; // e.g., "BiologicalMother", "AdoptiveFather", etc.
    public string? OtherNameType { get; set; } // If Type is Other, specify here
    public DateTime? EstablishedDate { get; set; } // When the relationship was established (e.g., adoption date)

}
