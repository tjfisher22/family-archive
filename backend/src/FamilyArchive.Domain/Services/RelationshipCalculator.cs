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

        var siblingType = commonParents > 1 ? "Full" : "Half";

        // Get gender-specific term, falling back gracefully
        var genderSpecificTerm = GetGenderSpecificTerm(b.Gender, "Sister", "Brother");

        return genderSpecificTerm != null
            ? $"{siblingType} {genderSpecificTerm}"
            : $"{siblingType} sibling";
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
    private RelationshipPath? FindRelationshipPath(Member memberA, Member memberB)
    {
        // TODO: Implement bidirectional BFS to find the Lowest Common Ancestor (LCA)
        // 
        // High-level approach:
        // 1. Use BFS to explore ALL ancestors of memberA (handling multiple parents)
        //    - Store visited nodes with their parent pointers (for path reconstruction)
        //    - Use a Queue to process nodes level by level
        // 
        // 2. Use BFS to explore ALL ancestors of memberB
        //    - When a node is found that's already visited from memberA search, that's the LCA
        //    - Stop search once LCA is found
        // 
        // 3. Reconstruct the path:
        //    - Backtrack from memberA to LCA using parent pointers
        //    - Backtrack from memberB to LCA using parent pointers
        //    - Combine: memberA ? LCA ? memberB (handling the LCA duplication)
        // 
        // 4. Return a RelationshipPath object with all steps and the CommonAncestor set

        throw new NotImplementedException();
    }

    private string DescribeRelationship(RelationshipPath path, Member memberA, Member memberB)
    {
        // TODO: Convert the relationship path into a human-readable description
        // 
        // Approach:
        // 1. Extract gender sequence from path (e.g., "MFM")
        //    - Use path.Steps to get genders of each node in the path
        // 
        // 2. Determine direction for each step (Up to LCA or Down from LCA)
        //    - Find LCA index in the steps list
        //    - Steps before LCA = "Up", steps after LCA = "Down"
        // 
        // 3. Look up the relationship name based on:
        //    - Gender sequence
        //    - Direction sequence (Up/Down pattern)
        //    - The gender of memberB (for gender-specific terms like "sister" vs "brother")
        // 
        // 4. Handle edge cases:
        //    - Non-binary genders (fallback to neutral terms)
        //    - In-law relationships (if path crosses partnerships)
        //    - Cultural variations (different kinship systems)
        // 
        // 5. Return the formatted relationship string

        throw new NotImplementedException();
    }

    // TODO: Create a private helper method for BFS ancestor exploration
    // private BfsAncestorResult GetAncestorsBfs(Member startMember)
    // {
    //     // This method should:
    //     // 1. Initialize a Queue with the start member
    //     // 2. Initialize a visited Dictionary to track: memberId -> BfsNode (with parent pointer)
    //     // 3. Perform BFS:
    //     //    - Dequeue current node
    //     //    - Mark as visited
    //     //    - Explore all parent relationships (Relationships collection)
    //     //    - Enqueue unvisited parents, storing their parent pointer for reconstruction
    //     // 4. Return the visited dictionary for LCA matching
    //     //
    //     // Note: Handle multiple parents per member (polyamory)
    // }

    // TODO: Create a private helper class/record for BFS tracking
    // private class BfsNode
    // {
    //     public Member Member { get; set; }
    //     public BfsNode? Parent { get; set; }  // BFS tree parent (for path reconstruction)
    // }
}