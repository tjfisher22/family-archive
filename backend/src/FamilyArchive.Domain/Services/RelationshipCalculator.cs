using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        var connection = FindRelationshipPath(memberA, memberB);
        if (connection == null)
            return "Not related";

        return DescribeRelationship(connection, memberA, memberB);
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
    private RelationshipConnection? FindRelationshipPath(Member memberA, Member memberB)
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

        return null; // Placeholder until implemented
    }

    private string DescribeRelationship(RelationshipConnection connection, Member memberA, Member memberB)
    {

        //Beter approach************
        var shortestDistance = Math.Min(connection.GenerationsUpFromA, connection.GenerationsUpFromB);
        var generationDifference = Math.Abs(connection.GenerationsUpFromA - connection.GenerationsUpFromB);

        string? completeTerm = null;

        if (shortestDistance == 1)
        {
            if (generationDifference == 0) //Should be resolved prior, but just in case
            {
                // Siblings
                return GetSiblingDescription(memberA, memberB);
            }
            else if (generationDifference >= 1)
            {

                string generationTerm = $"{GetGeneration(generationDifference)} {(generationDifference > 1 ? "Great" : "")}";

                // Aunt/Uncle or Niece/Nephew
                if (connection.GenerationsUpFromA < connection.GenerationsUpFromB)
                {
                    // memberA is Aunt/Uncle of memberB
                    var genderedTerm = GetGenderSpecificTerm(memberA.Gender, "Aunt", "Uncle") ?? "Pibling";
                    completeTerm = generationTerm + genderedTerm;

                }
                else
                {
                    // memberA is Niece/Nephew of memberB
                    var genderedTerm = GetGenderSpecificTerm(memberB.Gender, "Niece", "Nephew") ?? "Nibling";
                    completeTerm = generationTerm + genderedTerm;
                }
            }
        }
        else
        {
            // Cousins
            var cousinDegree = shortestDistance - 1;
            string ordinalTerm = GetOrdinal(cousinDegree);
            string generationTerm = $"{GetGeneration(generationDifference)} Removed";

            var genderedTerm = GetGenderSpecificTerm(memberA.Gender, "Cousin", "Cousin") ?? "Cousin"; //Included for translation purposes

            completeTerm = ordinalTerm + generationTerm + genderedTerm;
        }

        if (completeTerm != null)
            return completeTerm;
        return "Not related";

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
    private string GetGeneration(int number)
    {
        return number switch
        {
            0 => "",
            //1 => "",
            _ => $"{number} x"
        };
    }
    private string GetOrdinal(int number)
    {
        int lastDigit = number % 10;
        int lastTwoDigits = number % 100;
        
        if (lastTwoDigits >= 11 && lastTwoDigits <= 13)
            return $"{number}th";
        
        return lastDigit switch
        {
            1 => $"{number}st",
            2 => $"{number}nd",
            3 => $"{number}rd",
            _ => $"{number}th"
        };
    }
}