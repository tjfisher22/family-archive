using System;
using System.Collections.Generic;

namespace FamilyArchive.Application.DTOs;

public class MemberDto
{
    public Guid Id { get; set; }
    public DateTime? BirthDate { get; set; }
    public DateTime? DeathDate { get; set; }
    public string? Gender { get; set; }
    public Guid? FamilyId { get; set; }
    public List<MemberNameDto> Names { get; set; } = new();
}