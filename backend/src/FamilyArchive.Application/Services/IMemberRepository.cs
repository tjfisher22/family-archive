using FamilyArchive.Domain.Entities;
using System;

namespace FamilyArchive.Application.Services;

public interface IMemberRepository
{
    void AddMember(Member member);
    void UpdateMember(Member member);
    void RemoveMember(Member member);
    Member? GetMemberById(Guid memberId);
    IEnumerable<Member> GetAllMembers();
    void SaveChanges();
}
