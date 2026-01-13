using FamilyArchive.Domain.Entities;
using FamilyArchive.Domain.Enums;

namespace FamilyArchive.Application.Services;

public class MemberRelationshipService : IMemberRelationshipService
{
    private readonly IMemberRepository _repository;

    public MemberRelationshipService(IMemberRepository repository)
    {
        _repository = repository;
    }

    public void AddChildToMember(Guid parentId, Guid childId, RelationshipType relationshipType, string? otherRelationshipType, DateTime? establishedDate)
    {
        var parent = GetMember(parentId);
        var child = GetMember(childId);

        parent.AddChild(child, relationshipType, otherRelationshipType, establishedDate);
        _repository.UpdateMember(parent);
    }

    public void AddPartnerToMember(Guid memberId, Guid partnerId, PartnershipType partnershipType, string? otherPartnershipType, DateTime? startDate)
    {
        var member = GetMember(memberId);
        var partner = GetMember(partnerId);

        member.AddPartner(partner, partnershipType, otherPartnershipType, startDate);
        _repository.UpdateMember(member);
    }

    public void EndPartnership(Guid memberId, Guid partnershipId, DateTime endDate)
    {
        var member = GetMember(memberId);
        member.EndPartnership(partnershipId, endDate);
        _repository.UpdateMember(member);
    }

    public void SaveChanges()
    {
        _repository.SaveChanges();
    }

    private Member GetMember(Guid memberId)
    {
        var member = _repository.GetMemberById(memberId);
        if (member == null) throw new InvalidOperationException("Member not found");
        return member;
    }
}
