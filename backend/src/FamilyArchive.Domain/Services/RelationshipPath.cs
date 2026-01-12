using FamilyArchive.Domain.Entities;
using FamilyArchive.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyArchive.Domain.Services;

public class RelationshipPath
{
    public List<RelationshipStep> Steps { get; } = new List<RelationshipStep>();
    public void AddStep(RelationshipStep step)
    {
        ArgumentNullException.ThrowIfNull(step);
        ArgumentNullException.ThrowIfNull(step.Member, nameof(step.Member));
        
        Steps.Add(step);
    }
    public void ReverseSteps()
    {
        Steps.Reverse();
    }
    public Member? CommonAncestor { get; set; } = default!; //End Node
}

public class RelationshipStep
{
    public Member Member { get; set; } = default!;
    public Gender Gender => Member.Gender;
    // Direction is implicit: it should always be "up" towards ancestors
}
