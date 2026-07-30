using FamilyArchive.Domain.Enums;

namespace FamilyArchive.Application.Services;

public interface IMemberRelationshipService
{
    void AddChildToMember(Guid parentId, Guid childId, RelationshipType relationshipType, string? otherRelationshipType, DateTime? establishedDate);
    void RemoveChildFromMember(Guid parentId, Guid childId);
    void AddPartnerToMember(Guid memberId, Guid partnerId, PartnershipType partnershipType, string? otherPartnershipType, DateTime? startDate);
    void EndPartnership(Guid memberId, Guid partnershipId, DateTime endDate);
    void SaveChanges();
}
