using FamilyArchive.Application.DTOs;
using FamilyArchive.Domain.Enums;

namespace FamilyArchive.Application.Services;

public interface IMemberNameService
{
    void AddNameToMember(Guid memberId, MemberNameDto dto);
    void UpdateNameOfMember(Guid memberId, Guid nameId, string newName);
    void UpdateNameOrderOfMember(Guid memberId, Guid nameId, int newOrder);
    void UpdateNameTypeOfMember(Guid memberId, Guid nameId, NameType? newType, string? otherNameType);
    void UpdateNameHiddenOfMember(Guid memberId, Guid nameId, bool hidden);
    void SaveChanges();
}
