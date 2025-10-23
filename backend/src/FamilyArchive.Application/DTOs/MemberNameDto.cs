using System;
using FamilyArchive.Domain.Enums;

namespace FamilyArchive.Application.DTOs;
public class MemberNameDto
{
    public Guid Id { get; set; }
    public string Value { get; set; } = default!;
    public NameType? Type { get; set; }
    public string? OtherNameType { get; set; }
    public int Order { get; set; }
    public bool Hidden { get; set; }
}

