using FamilyArchive.Domain.Enums;

namespace FamilyArchive.Api.Controllers;

public class AddPartnerRequest
{
    public Guid PartnerId { get; set; }
    public PartnershipType PartnershipType { get; set; }
    public string? OtherPartnershipType { get; set; }
    public DateTime? StartDate { get; set; }
}