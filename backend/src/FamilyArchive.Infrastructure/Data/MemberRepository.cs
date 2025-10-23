using FamilyArchive.Application.Services;
using FamilyArchive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;

namespace FamilyArchive.Infrastructure.Data;

public class MemberRepository : IMemberRepository
{
    private readonly FamilyArchiveDbContext _context;

    public MemberRepository(FamilyArchiveDbContext context)
    {
        _context = context;
    }

    public void AddMember(Member member)
    {
        _context.Members.Add(member);
    }

    public void UpdateMember(Member member)
    {
        _context.Members.Update(member);
    }
    public void RemoveMember(Member member)
    {
        _context.Members.Remove(member);
    }
    
    public Member? GetMemberById(Guid memberId)
    {
        return _context.Members
            .Include(m => m.Relationships)
            .Include(m => m.ParentRelationships)
            .Include(m => m.Partnerships)
            .Include(m => m.PartnerPartnerships)
            .Include(m => m.Family)
            .FirstOrDefault(m => m.Id == memberId);
    }


    public void SaveChanges()
    {
         _context.SaveChangesAsync();
    }
}