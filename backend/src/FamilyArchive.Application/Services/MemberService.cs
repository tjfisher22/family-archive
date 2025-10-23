using FamilyArchive.Domain.Entities;
using FamilyArchive.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace FamilyArchive.Application.Services;

public interface IMemberService
{
    void AddName(Member member, MemberName name);
    void RemoveName(Member member, Guid memberNameId);
    Member? GetMemberById(Guid memberId);
    void SaveMemberChanges();
    //void DeleteMember(Member member);
}

public class MemberService : IMemberService
{
    private readonly FamilyArchiveDbContext _context;

    public MemberService(FamilyArchiveDbContext context)
    {
        _context = context;
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
}
