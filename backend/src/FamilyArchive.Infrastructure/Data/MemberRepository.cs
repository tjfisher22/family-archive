using FamilyArchive.Domain.Entities;
using FamilyArchive.Application.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FamilyArchive.Infrastructure.Data;

public class MemberRepository : IMemberRepository
{
    private readonly FamilyArchiveDbContext _context;

    public MemberRepository(FamilyArchiveDbContext context)
    {
        _context = context;
    }

    public async Task AddMemberAsync(Member member)
    {
        await _context.Members.AddAsync(member);
    }

    public async Task UpdateMemberAsync(Member member)
    {
        _context.Members.Update(member);
        await Task.CompletedTask;
    }
    public async Task RemoveMemberAsync(Member member)
    {
        _context.Members.Remove(member);
        await Task.CompletedTask;
    }
    public async Task<Member?> GetMemberByIdAsync(Guid memberId)
    {
        return await _context.Members
            .Include(m => m.Relationships)
            .Include(m => m.ParentRelationships)
            .Include(m => m.Partnerships)
            .Include(m => m.PartnerPartnerships)
            .Include(m => m.Family)
            .FirstOrDefaultAsync(m => m.Id == memberId);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}