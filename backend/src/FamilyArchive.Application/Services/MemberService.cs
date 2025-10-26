using FamilyArchive.Application.DTOs;
using FamilyArchive.Domain.Entities;
using FamilyArchive.Domain.Enums;
using System;
using System.Diagnostics.Metrics;
using System.Linq;

namespace FamilyArchive.Application.Services;

public interface IMemberService
{
    Guid AddMemberFromDto(MemberDto dto);
    MemberDto GetMemberById(Guid memberId);
    void UpdateMemberById(Guid memberId, MemberDto dto);
    void AddNameToMember(Guid memberId, MemberNameDto dto);
    void UpdateNameOfMember(Guid memberId, Guid nameId, string NewName);
    void SaveMemberChanges();
    void UpdateNameOrderOfMember(Guid memberId, Guid nameId, int newOrder);
    void UpdateNameTypeOfMember(Guid memberId, Guid nameId, NameType? newType, string? OtherNameType);
    void UpdateNameHiddenOfMember(Guid memberId, Guid nameId, bool hidden);
}

public class MemberService : IMemberService
{
    private readonly IMemberRepository _repository;

    public MemberService(IMemberRepository repository)
    {
        _repository = repository;
    }
    public Guid AddMemberFromDto(MemberDto dto)
    {
        var member = new Member
        {
            BirthDate = dto.BirthDate,
            DeathDate = dto.DeathDate,
            Gender = dto.Gender,
            FamilyId = dto.FamilyId
        };
        _repository.AddMember(member);
        return member.Id;
    }
    public MemberDto GetMemberById(Guid memberId)
    {
        var member = _repository.GetMemberById(memberId);
        if (member == null) throw new InvalidOperationException("Member not found");
        return ToDto(member);
    }

    public void SaveMemberChanges()
    {
        _repository.SaveChanges();
    }

    public void UpdateMemberById(Guid memberId, MemberDto dto)
    {
       
        var member = _repository.GetMemberById(memberId);
        if (member == null) throw new InvalidOperationException("Member not found");
        member.BirthDate = dto.BirthDate;
        member.DeathDate = dto.DeathDate;
        member.FamilyId = dto.FamilyId;
        _repository.UpdateMember(member);
    }

    public void AddNameToMember(Guid memberId, MemberNameDto dto)
    {
        var member = _repository.GetMemberById(memberId);
        if (member == null) throw new InvalidOperationException("Member not found");

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

    public void UpdateNameTypeOfMember(Guid memberId, Guid nameId, NameType? newType, string? OtherNameType)
    {
        var member = GetMember(memberId);
        member.UpdateNameType(nameId, newType, OtherNameType);
        _repository.UpdateMember(member);
    }

    public void UpdateNameHiddenOfMember(Guid memberId, Guid nameId, bool hidden)
    {
        var member = GetMember(memberId);
        member.UpdateNameHidden(nameId, hidden);
        _repository.UpdateMember(member);
    }

    private Member GetMember(Guid memberId)
    {
        var member = _repository.GetMemberById(memberId);
        if (member == null) throw new InvalidOperationException("Member not found");
        return member;
    }
    private MemberDto ToDto(Member member)
    {
        return new MemberDto
        {
            Id = member.Id,
            BirthDate = member.BirthDate,
            DeathDate = member.DeathDate,
            Gender = member.Gender,
            FamilyId = member.FamilyId,
            Names = member.Names.Select(n => new MemberNameDto
            {
                Id = n.Id,
                Value = n.Value,
                Type = n.Type,
                OtherNameType = n.OtherNameType,
                Order = n.Order,
                Hidden = n.Hidden
            }).ToList()
        };
    }
}
