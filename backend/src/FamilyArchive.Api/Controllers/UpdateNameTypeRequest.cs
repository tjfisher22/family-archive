namespace FamilyArchive.Api.Controllers;
using FamilyArchive.Domain.Enums;

public class UpdateNameTypeRequest
{
    public NameType NewType { get; set; }
    public string? OtherNameType { get; set; }
}