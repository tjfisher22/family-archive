using FamilyArchive.Application.DTOs;
using FamilyArchive.Domain.Entities;
using System;
using System.Linq;

namespace FamilyArchive.Application.Services;

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

    public IEnumerable<MemberDto> GetAllMembers()
    {
        var members = _repository.GetAllMembers();
        return members.Select(ToDto);
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

    public void SaveChanges()
    {
        _repository.SaveChanges();
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
