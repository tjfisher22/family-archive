using FamilyArchive.Application.DTOs;
using FamilyArchive.Domain.Entities;
using FamilyArchive.Domain.Enums;
using System;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyArchive.Application.Services;

public interface IMemberService
{
    Task<Guid> AddMemberFromDtoAsync(MemberDto dto);
    Task<MemberDto> GetMemberByIdAsync(Guid memberId);
    Task UpdateMemberByIdAsync(Guid memberId, MemberDto dto);
    Task AddNameToMemberAsync(Guid memberId, MemberNameDto dto);
    Task UpdateNameOfMemberAsync(Guid memberId, Guid nameId, string NewName);
    Task SaveMemberChangesAsync();
    Task UpdateNameOrderOfMemberAsync(Guid memberId, Guid nameId, int newOrder);
    Task UpdateNameTypeOfMemberAsync(Guid memberId, Guid nameId, NameType? newType, string? OtherNameType);
    Task UpdateNameHiddenOfMemberAsync(Guid memberId, Guid nameId, bool hidden);
}

public class MemberService : IMemberService
{
    private readonly IMemberRepository _repository;

    public MemberService(IMemberRepository repository)
    {
        _repository = repository;
    }
    public async Task<Guid> AddMemberFromDtoAsync(MemberDto dto)
    {
        var member = new Member
        {
            BirthDate = dto.BirthDate,
            DeathDate = dto.DeathDate,
            Gender = dto.Gender,
            FamilyId = dto.FamilyId
        };
        await _repository.AddMemberAsync(member);
        return member.Id;
    }
    public async Task<MemberDto> GetMemberByIdAsync(Guid memberId)
    {
        var member = await _repository.GetMemberByIdAsync(memberId);
        if (member == null) throw new InvalidOperationException("Member not found");
        return ToDto(member);
    }

    public async Task SaveMemberChangesAsync()
    {
        await _repository.SaveChangesAsync();
    }

    public async Task UpdateMemberByIdAsync(Guid memberId, MemberDto dto)
    {
        var member = await _repository.GetMemberByIdAsync(memberId);
        if (member == null) throw new InvalidOperationException("Member not found");
        member.BirthDate = dto.BirthDate;
        member.DeathDate = dto.DeathDate;
        member.FamilyId = dto.FamilyId;
        await _repository.UpdateMemberAsync(member);
    }

    public async Task AddNameToMemberAsync(Guid memberId, MemberNameDto dto)
    {
        var member = await _repository.GetMemberByIdAsync(memberId);
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
        await _repository.UpdateMemberAsync(member);
    }

    public async Task UpdateNameOfMemberAsync(Guid memberId, Guid nameId, string newName)
    {
        var member = await GetMemberAsync(memberId);
        member.UpdateNameValue(nameId, newName);
        await _repository.UpdateMemberAsync(member);
    }
    public async Task UpdateNameOrderOfMemberAsync(Guid memberId, Guid nameId, int newOrder)
    {
        var member = await GetMemberAsync(memberId);
        member.UpdateNameOrder(nameId, newOrder);
        await _repository.UpdateMemberAsync(member);
    }

    public async Task UpdateNameTypeOfMemberAsync(Guid memberId, Guid nameId, NameType? newType, string? OtherNameType)
    {
        var member = await GetMemberAsync(memberId);
        member.UpdateNameType(nameId, newType, OtherNameType);
        await _repository.UpdateMemberAsync(member);
    }

    public async Task UpdateNameHiddenOfMemberAsync(Guid memberId, Guid nameId, bool hidden)
    {
        var member = await GetMemberAsync(memberId);
        member.UpdateNameHidden(nameId, hidden);
        await _repository.UpdateMemberAsync(member);
    }

    private async Task<Member> GetMemberAsync(Guid memberId)
    {
        var member = await _repository.GetMemberByIdAsync(memberId);
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
