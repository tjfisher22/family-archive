using FamilyArchive.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace FamilyArchive.Application.Services;

public interface IMemberRepository
{
    Task AddMemberAsync(Member member);
    Task UpdateMemberAsync(Member member);
    Task<Member?> GetMemberByIdAsync(Guid memberId);
    Task SaveChangesAsync();
}
