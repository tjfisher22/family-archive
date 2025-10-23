using FamilyArchive.Application.DTOs;
using FamilyArchive.Domain.Entities;
using FamilyArchive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace FamilyArchive.Application.Services;

public interface IMemberService
{
    void AddMember(Member member);
    void UpdateMember(Member member, MemberDto dto);
    void AddName(Member member, MemberName name);
    void RemoveName(Member member, Guid memberNameId);
    void UpdateName(Member member, MemberNameDto dto);
    Member? GetMemberById(Guid memberId);
    void SaveMemberChanges();
    void AddChild(Member parent, Member child, string relationshipType, DateTime? establishedDate = null);

    //void UpdateBirthDate(Member member, DateTime? birthDate);
    //void UpdateDeathDate(Member member, DateTime? deathDate);
    //void DeleteMember(Member member);
}

public class MemberService : IMemberService
{
    private readonly FamilyArchiveDbContext _context;

    public MemberService(FamilyArchiveDbContext context)
    {
        _context = context;
    }
    public void AddMember(Member member)
    {
        _context.Members.Add(member);
    }
    public void UpdateMember(Member member, MemberDto dto)
    {
        member.BirthDate = dto.BirthDate;
        member.DeathDate = dto.DeathDate;
        member.Gender = dto.Gender;
        member.FamilyId = dto.FamilyId;
    }
    public void AddName(Member member, MemberName name)
    {
        member.AddName(name);
    }
    public void RemoveName(Member member, Guid memberNameId)
    {
        member.RemoveName(memberNameId);
    }
    public Member? GetMemberById(Guid memberId)
    {
        // Includes related names, relationships, and partnerships for a complete member object
        return _context.Members
            .Include(m => m.Relationships)
            .Include(m => m.ParentRelationships)
            .Include(m => m.Partnerships)
            .Include(m => m.PartnerPartnerships)
            .Include(m => m.Family)
            .FirstOrDefault(m => m.Id == memberId);
    }
    public void SaveMemberChanges()
    {
        _context.SaveChanges();
    }

    public void AddChild(Member parent, Member child, string relationshipType, DateTime? establishedDate = null)
    {
        parent.AddChild(child, relationshipType, establishedDate);
        // Save changes to the database if needed
        _context.Update(parent);
        _context.Update(child);
    }

    //public void UpdateBirthDate(Member member, DateTime? birthDate)
    //{
    //    member.BirthDate = birthDate;
    //    _context.Update(member);
    //}

    //public void UpdateDeathDate(Member member, DateTime? deathDate)
    //{
    //    member.DeathDate = deathDate;
    //    _context.Update(member);
    //}
    public void UpdateName(Member member, MemberNameDto dto)
    {
        member.UpdateName(dto.Id, dto.Value, dto.Type, dto.OtherNameType, dto.Order, dto.Hidden);
        _context.Update(member);
    }
}
