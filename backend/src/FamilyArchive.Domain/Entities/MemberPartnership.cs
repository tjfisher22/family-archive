using FamilyArchive.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyArchive.Domain.Entities;

public class MemberPartnership
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; } // One partner (from the family)
    public Member Member { get; set; } = default!;
    public Guid PartnerId { get; set; } // The other partner (from outside the family?)
    public Member Partner { get; set; } = default!;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public PartnershipType PartnershipType { get; set; } // e.g., "Marriage", "CivilUnion", "DomesticPartnership", etc.
}
