using System;
using System.Collections.Generic;
using System.Linq;
using FamilyArchive.Domain.Entities;
using FamilyArchive.Domain.Enums;

namespace FamilyArchive.Domain.Services;

public class RelationshipCalculator
{
    /// <summary>
    /// Calculates the familial relationship between two members
    /// </summary>
    public string CalculateRelationship(Member memberA, Member memberB)
    {
        if (memberA.Id == memberB.Id)
            return "Self";

        // Check if they're partners
        if (IsPartner(memberA, memberB))
            return GetPartnershipDescription(memberA, memberB);

        // Check direct parent-child
        if (IsDirectChild(memberA, memberB))
            return "Child";
        if (IsDirectChild(memberB, memberA))
            return GetParentDescription(memberB, memberA);

        // Check siblings
        if (AreSiblings(memberA, memberB))
            return GetSiblingDescription(memberA, memberB);

        // For more complex relationships, find the path
        var path = FindRelationshipPath(memberA, memberB);
        if (path == null)
            return "Not related";

        return DescribeRelationship(path, memberA, memberB);
    }



    private bool IsPartner(Member a, Member b)
    {
        return a.Partnerships.Any(p => p.PartnerId == b.Id) ||
               a.PartnerPartnerships.Any(p => p.MemberId == b.Id);
    }

    private bool IsDirectChild(Member parent, Member child)
    {
        return parent.ParentRelationships.Any(r => r.MemberId == child.Id);
    }

    private bool AreSiblings(Member a, Member b)
    {
        var aParents = a.Relationships.Select(r => r.RelatedMemberId).ToHashSet();
        var bParents = b.Relationships.Select(r => r.RelatedMemberId).ToHashSet();
        return aParents.Intersect(bParents).Any();
    }

    private string GetParentDescription(Member parent, Member child)
    {
        var relationship = child.Relationships.FirstOrDefault(r => r.RelatedMemberId == parent.Id);
        if (relationship == null)
            return "Parent";

        return relationship.RelationshipType.GetDisplayName();
    }

    private string GetSiblingDescription(Member a, Member b)
    {
        // Check if full sibling (both parents match) or half-sibling
        var aParents = a.Relationships.Select(r => r.RelatedMemberId).ToHashSet();
        var bParents = b.Relationships.Select(r => r.RelatedMemberId).ToHashSet();
        var commonParents = aParents.Intersect(bParents).Count();

        var siblingType = commonParents > 1 ? "Full Sibling" : "Half-Sibling";

        // Get gender-specific term, falling back gracefully
        var genderSpecificTerm = GetGenderSpecificTerm(b.Gender, "Sister", "Brother");
        
        return genderSpecificTerm != null 
            ? $"{siblingType} ({genderSpecificTerm})" 
            : siblingType;
    }

    private string? GetGenderSpecificTerm(Member member, string femaleLabel, string maleLabel)
    {
        return GetGenderSpecificTerm(member.Gender, femaleLabel, maleLabel);
    }

    private string? GetGenderSpecificTerm(Gender gender, string femaleLabel, string maleLabel)
    {
        return gender switch
        {
            Gender.Female => femaleLabel,
            Gender.Male => maleLabel,
            _ => null  // Explicitly handle unknown genders
        };
    }

    private string GetPartnershipDescription(Member a, Member b)
    {
        var partnership = a.Partnerships.FirstOrDefault(p => p.PartnerId == b.Id) ??
                          a.PartnerPartnerships.FirstOrDefault(p => p.MemberId == b.Id);

        if (partnership == null)
            return "Partner";

        if(partnership.PartnershipType == PartnershipType.Marriage)
            return GetGenderSpecificTerm(b, "Wife", "Husband") ?? "Spouse";
        else
            return partnership.PartnershipType.GetDisplayName();
    }
    //TODO: Implement FindRelationshipPath and DescribeRelationship for complex relationships
    private object FindRelationshipPath(Member memberA, Member memberB)
    {
        throw new NotImplementedException();
    }
    private string DescribeRelationship(object path, Member memberA, Member memberB)
    {
        throw new NotImplementedException();
    }
}