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
    /// Calculates the closest familial relationship between two members
    /// </summary>
    public string CalculateClosestRelationship(Member memberA, Member memberB)
    {
        var closeRelationship = GetCloseRelationship(memberA, memberB);
        if (closeRelationship != null)
            return closeRelationship;

        // For more complex relationships, find all paths and use the closest
        var connections = FindAllRelationshipPaths(memberA, memberB);
        if (connections.Count == 0)
            return "Not related";

        var closestConnection = connections[0]; // Already sorted by shortest path
        return DescribeRelationship(closestConnection, memberA, memberB);
    }

    /// <summary>
    /// Calculates all familial relationships between two members, sorted by shortest path first
    /// </summary>
    public List<string> CalculateAllRelationships(Member memberA, Member memberB)
    {
        var closeRelationship = GetCloseRelationship(memberA, memberB);
        if (closeRelationship != null)
            return new List<string> { closeRelationship };

        // For more complex relationships, find all paths
        var connections = FindAllRelationshipPaths(memberA, memberB);
        if (connections.Count == 0)
            return new List<string> { "Not related" };

        return connections
            .Select(conn => DescribeRelationship(conn, memberA, memberB))
            .ToList();
    }

    /// <summary>
    /// Checks for close relationships (self, partners, direct parent-child, and siblings).
    /// Returns the relationship description if found, null otherwise.
    /// </summary>
    private string? GetCloseRelationship(Member memberA, Member memberB)
    {
        if (memberA.Id == memberB.Id)
            return "Self";

        // Check if they're partners
        if (IsPartner(memberA, memberB))
            return GetPartnershipDescription(memberA, memberB);

        // Check direct parent-child
        if (IsDirectChild(memberA, memberB))
            return GetGenderSpecificTerm(memberB,"Daughter","Son") ?? "Child";
        if (IsDirectChild(memberB, memberA))
            return GetParentDescription(memberB, memberA);

        // Check siblings
        if (AreSiblings(memberA, memberB))
            return GetSiblingDescription(memberA, memberB);

        return null;
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
            : $"{siblingType} Sibling";
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

    /// <summary>
    /// Finds all relationship paths between two members through their common ancestors.
    /// Paths are sorted by shortest total distance first.
    /// </summary>
    private List<RelationshipConnection> FindAllRelationshipPaths(Member memberA, Member memberB)
    {
        var ancestorsA = GetAncestorsBfs(memberA);
        var ancestorsB = GetAncestorsBfs(memberB);

        var commonAncestorIds = ancestorsA.Keys.Intersect(ancestorsB.Keys).ToList();
        
        if (commonAncestorIds.Count == 0)
            return new List<RelationshipConnection>();

        var connections = new List<RelationshipConnection>();

        foreach (var ancestorId in commonAncestorIds)
        {
            var pathA = ReconstructPath(ancestorId, ancestorsA);
            var pathB = ReconstructPath(ancestorId, ancestorsB);

            if (pathA?.Steps.Count > 0 && pathB?.Steps.Count > 0)
            {
                var commonAncestor = pathA.CommonAncestor;
                var connection = new RelationshipConnection
                {
                    PathFromMemberA = pathA,
                    PathFromMemberB = pathB,
                    CommonAncestor = commonAncestor
                };
                connections.Add(connection);
            }
        }

        // Sort by shortest combined path first (sum of generations up)
        return connections
            .OrderBy(c => c.GenerationsUpFromA + c.GenerationsUpFromB)
            .ThenBy(c => Math.Abs(c.GenerationsUpFromA - c.GenerationsUpFromB))
            .ToList();
    }

    /// <summary>
    /// Performs BFS from a starting member to find all ancestors and their paths.
    /// Returns a dictionary mapping ancestor member IDs to BFS nodes for path reconstruction.
    /// </summary>
    private Dictionary<Guid, BfsNode> GetAncestorsBfs(Member startMember)
    {
        var visited = new Dictionary<Guid, BfsNode>();
        var queue = new Queue<BfsNode>();

        var startNode = new BfsNode { Member = startMember, Parent = null };
        queue.Enqueue(startNode);
        visited[startMember.Id] = startNode;

        while (queue.Count > 0)
        {
            var currentNode = queue.Dequeue();
            
            // Explore all parents of the current member
            var parents = currentNode.Member.Relationships.Select(r => r.RelatedMember).Where(m => m != null).ToList();
            
            foreach (var parent in parents)
            {
                if (parent != null && !visited.ContainsKey(parent.Id))
                {
                    var parentNode = new BfsNode { Member = parent, Parent = currentNode };
                    visited[parent.Id] = parentNode;
                    queue.Enqueue(parentNode);
                }
            }
        }

        return visited;
    }

    /// <summary>
    /// Reconstructs the path from a member to a specific ancestor using the BFS tracking nodes.
    /// </summary>
    private RelationshipPath? ReconstructPath(Guid ancestorId, Dictionary<Guid, BfsNode> visited)
    {
        if (!visited.ContainsKey(ancestorId))
            return null;

        var path = new RelationshipPath();
        var current = visited[ancestorId];

        // Traverse back through parent pointers to build the path
        while (current != null)
        {
            path.AddStep(new RelationshipStep { Member = current.Member });
            current = current.Parent;
        }

        // Reverse so the path goes from member towards ancestor
        path.ReverseSteps();
        
        // Set the common ancestor
        path.CommonAncestor = visited[ancestorId].Member;

        return path;
    }

    /// <summary>
    /// Helper class for BFS tracking during ancestor exploration.
    /// Stores a member and parent pointer for path reconstruction.
    /// </summary>
    private class BfsNode
    {
        public Member Member { get; set; } = default!;
        public BfsNode? Parent { get; set; }  // BFS tree parent (for path reconstruction)
    }

    private string DescribeRelationship(RelationshipConnection connection, Member memberA, Member memberB)
    {
        var shortestDistance = Math.Min(connection.GenerationsUpFromA, connection.GenerationsUpFromB);
        var generationDifference = Math.Abs(connection.GenerationsUpFromA - connection.GenerationsUpFromB);

        // Grandparent/Grandchild (direct line, 2+ generations apart)
        if (shortestDistance == 1 && generationDifference >= 2)
        {
            return DescribeGrandparentGrandchild(connection, memberA, memberB, generationDifference);
        }

        // Aunt/Uncle or Niece/Nephew (one person's parent is sibling of the other)
        if (shortestDistance == 2)
        {
            return DescribeAuntUncleNieceNephew(connection, memberA, memberB, generationDifference);
        }

        // Cousins (both descended from common ancestor through siblings)
        if (shortestDistance >= 2)
        {
            return DescribeCousin(shortestDistance, generationDifference);
        }

        return "Not related";
    }

    private string DescribeGrandparentGrandchild(RelationshipConnection connection, Member memberA, Member memberB, int generationDifference)
    {
        var generationPrefix = GetGenerationPrefix(generationDifference);

        if (connection.GenerationsUpFromA > connection.GenerationsUpFromB)
        {
            // memberA is the ancestor (grandparent, great-grandparent, etc.)
            return $"{generationPrefix}{GetGenderSpecificTerm(memberB,"mother","father")??"parent"}";
        }
        else
        {
            // memberA is the descendant (grandchild, great-grandchild, etc.)
            return $"{generationPrefix}{GetGenderSpecificTerm(memberB, "daughter", "son") ?? "child"}";
        }
    }

    private string DescribeAuntUncleNieceNephew(RelationshipConnection connection, Member memberA, Member memberB, int generationDifference)
    {
        var generationPrefix = GetGenerationPrefix(generationDifference);

        if (connection.GenerationsUpFromA > connection.GenerationsUpFromB)
        {
            // memberA is closer to ancestor, so memberA is the aunt/uncle of memberB
            var genderedTerm = GetGenderSpecificTerm(memberA, "Aunt", "Uncle") ?? "Pibling";
            return $"{generationPrefix}{genderedTerm}";
        }
        else
        {
            // memberB is closer to ancestor, so memberA is the niece/nephew of memberB
            var genderedTerm = GetGenderSpecificTerm(memberA, "Niece", "Nephew") ?? "Nibling";
            return $"{generationPrefix}{genderedTerm}";
        }
    }

    private string DescribeCousin(int shortestDistance, int generationDifference)
    {
        int cousinDegree = shortestDistance - 2; //-2 because we are including the common ancestor
        string ordinalTerm = GetOrdinalWord(cousinDegree);
        string generationSuffix = GetGenerationRemovedSuffix(generationDifference);
        
        return $"{ordinalTerm} Cousin{generationSuffix}";
    }

    private string GetGenerationRemovedSuffix(int generationDifference)
    {
        if (generationDifference == 0)
            return "";

        string removedTerm = generationDifference switch
        {
            1 => "Once",
            2 => "Twice",
            3 => "Thrice",
            _ => $"{generationDifference} Times"
        };

        return $" {removedTerm} Removed";
    }

    private string GetGenerationPrefix(int generationDifference)
    {
        return generationDifference switch
        {
            1 => "",
            2 => "Grand",
            3 => "Great Grand",
            4 => "Great Great Grand",
            _ => $"{GetOrdinalWord(generationDifference - 2)} Great Grand"
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

    private string GetOrdinalWord(int number)
    {
        return number switch
        {
            1 => "First",
            2 => "Second",
            3 => "Third",
            4 => "Fourth",
            5 => "Fifth",
            6 => "Sixth",
            7 => "Seventh",
            8 => "Eighth",
            9 => "Ninth",
            10 => "Tenth",
            _ => $"{GetOrdinal(number)}"
        };
    }
}