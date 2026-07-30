using FamilyArchive.Application.DTOs;
using FamilyArchive.Domain.Enums;

namespace FamilyArchive.Application.Services;

public interface IMemberService
{
    Guid AddMemberFromDto(MemberDto dto);
    MemberDto GetMemberById(Guid memberId);
    IEnumerable<MemberDto> GetAllMembers();
    void UpdateMemberById(Guid memberId, MemberDto dto);
    void RemoveMemberById(Guid memberId);
    void UpdateGenderOfMember(Guid memberId, Gender gender, string? otherGender);
    void SaveChanges();
}
