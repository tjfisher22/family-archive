using FamilyArchive.Domain.Entities;
using System;

namespace FamilyArchive.Application.Services;

public interface IMemberRepository
{
    void AddMember(Member member);
    void UpdateMember(Member member);
    Member? GetMemberById(Guid memberId);
    void SaveChanges();
}
