using FamilyArchive.Application.DTOs;
using FamilyArchive.Domain.Entities;
using FamilyArchive.Domain.Enums;

namespace FamilyArchive.Application.Services;

public class MemberNameService : IMemberNameService
{
    private readonly IMemberRepository _repository;

    public MemberNameService(IMemberRepository repository)
    {
        _repository = repository;
    }

    public void AddNameToMember(Guid memberId, MemberNameDto dto)
    {
        var member = GetMember(memberId);

        var name = new MemberName
        {
            Id = Guid.NewGuid(),
            MemberId = memberId,
            Value = dto.Value,
            Type = dto.Type,
            OtherNameType = dto.OtherNameType,
            Order = dto.Order,
            Hidden = dto.Hidden
        };

        member.AddName(name);
        _repository.UpdateMember(member);
    }

    public void UpdateNameOfMember(Guid memberId, Guid nameId, string newName)
    {
        var member = GetMember(memberId);
        member.UpdateNameValue(nameId, newName);
        _repository.UpdateMember(member);
    }

    public void UpdateNameOrderOfMember(Guid memberId, Guid nameId, int newOrder)
    {
        var member = GetMember(memberId);
        member.UpdateNameOrder(nameId, newOrder);
        _repository.UpdateMember(member);
    }

    public void UpdateNameTypeOfMember(Guid memberId, Guid nameId, NameType? newType, string? otherNameType)
    {
        var member = GetMember(memberId);
        member.UpdateNameType(nameId, newType, otherNameType);
        _repository.UpdateMember(member);
    }

    public void UpdateNameHiddenOfMember(Guid memberId, Guid nameId, bool hidden)
    {
        var member = GetMember(memberId);
        member.UpdateNameHidden(nameId, hidden);
        _repository.UpdateMember(member);
    }
    public void RemoveNameFromMember(Guid memberId, Guid nameId)
    {
        var member = GetMember(memberId);
        member.RemoveName(nameId);
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
