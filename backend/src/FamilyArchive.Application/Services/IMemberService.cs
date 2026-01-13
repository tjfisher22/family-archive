using FamilyArchive.Application.DTOs;

namespace FamilyArchive.Application.Services;

public interface IMemberService
{
    Guid AddMemberFromDto(MemberDto dto);
    MemberDto GetMemberById(Guid memberId);
    IEnumerable<MemberDto> GetAllMembers();
    void UpdateMemberById(Guid memberId, MemberDto dto);
    void SaveChanges();
}
