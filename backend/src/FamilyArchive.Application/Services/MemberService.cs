using FamilyArchive.Application.DTOs;
using FamilyArchive.Domain.Entities;
using System;
using System.Linq;

namespace FamilyArchive.Application.Services;

public interface IMemberService
{
    void AddMember(Member member);
    void UpdateMember(Member member, MemberDto dto);
    void AddName(Member member, MemberName name);
    void RemoveName(Member member, Guid memberNameId);
    void UpdateName(Member member, MemberNameDto dto);
    MemberDto? GetMemberById(Guid memberId);
    void SaveMemberChanges();
    void AddChild(Member parent, Member child, string relationshipType, DateTime? establishedDate = null);
}

public class MemberService : IMemberService
{
    private readonly IMemberRepository _repository;

    public MemberService(IMemberRepository repository)
    {
        _repository = repository;
    }
    public void AddMember(Member member)
    {
        _repository.AddMember(member);
    }
    public void UpdateMember(Member member, MemberDto dto)
    {
        member.BirthDate = dto.BirthDate;
        member.DeathDate = dto.DeathDate;
        member.Gender = dto.Gender;
        member.FamilyId = dto.FamilyId;
        _repository.UpdateMember(member);
    }
    public void AddName(Member member, MemberName name)
    {
        member.AddName(name);
        _repository.UpdateMember(member);
    }
    public void RemoveName(Member member, Guid memberNameId)
    {
        member.RemoveName(memberNameId);
        _repository.UpdateMember(member);
    }
    public MemberDto? GetMemberById(Guid memberId)
    {
        var member = _repository.GetMemberById(memberId);
        if (member == null) return null;
        return ToDto(member);
    }
    public void SaveMemberChanges()
    {
        _repository.SaveChanges();
    }

    public void AddChild(Member parent, Member child, string relationshipType, DateTime? establishedDate = null)
    {
        parent.AddChild(child, relationshipType, establishedDate);
        _repository.UpdateMember(parent);
        _repository.UpdateMember(child);
    }

    public void UpdateName(Member member, MemberNameDto dto)
    {
        member.UpdateName(dto.Id, dto.Value, dto.Type, dto.OtherNameType, dto.Order, dto.Hidden);
        _repository.UpdateMember(member);
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
