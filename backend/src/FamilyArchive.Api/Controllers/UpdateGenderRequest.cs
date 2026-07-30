using FamilyArchive.Domain.Enums;

namespace FamilyArchive.Api.Controllers;

public class UpdateGenderRequest
{
    public Gender Gender { get; set; }
    public string? OtherGender { get; set; }
}