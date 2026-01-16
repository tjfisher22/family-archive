using FamilyArchive.Domain.Enums;

namespace FamilyArchive.Api.Controllers;

public class AddChildRequest
{
    public Guid childId { get; set; }
    public RelationshipType RelationshipType { get; set; }
    public string? OtherRelationshipType { get; set; }
    public DateTime? EstablishedDate { get; set; }
}