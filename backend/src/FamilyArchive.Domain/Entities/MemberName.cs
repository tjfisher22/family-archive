using FamilyArchive.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace FamilyArchive.Domain.Entities;

public class MemberName
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public Member Member { get; set; } = default!;

    public string Value { get; set; } = default!; // The actual name, supports Unicode by default in .NET
    public NameType? Type { get; set; } // Enum: FirstName, MiddleName, LastName, ChosenName, Nickname, etc.
    public int Order { get; set; } // Display order
    public bool Hidden { get; set; } // Hide from display if true
}

