using FamilyArchive.Domain.Entities;
using FamilyArchive.Domain.Enums;
using System;

namespace FamilyArchive.UnitTests;

public class MemberNameTests
{
    [Fact]
    public void CanCreateMemberName_WithAllProperties()
    {
        var member = new Member { Id = Guid.NewGuid() };
        var name = new MemberName
        {
            Id = Guid.NewGuid(),
            Member = member,
            MemberId = member.Id,
            Value = "Aragorn",
            Type = NameType.ChosenName,
            OtherNameType = null,
            Order = 1,
            Hidden = false
        };

        Assert.Equal("Aragorn", name.Value);
        Assert.Equal(NameType.ChosenName, name.Type);
        Assert.Null(name.OtherNameType);
        Assert.False(name.Hidden);
        Assert.Equal(member.Id, name.MemberId);
        Assert.Equal(member, name.Member);
    }

    [Fact]
    public void Adding_Duplicate_Order_Should_Throw()
    {
        var member = new Member { Id = Guid.NewGuid() };
        var name1 = new MemberName
        {
            Id = Guid.NewGuid(),
            Member = member,
            MemberId = member.Id,
            Value = "Aragorn",
            Type = NameType.ChosenName,
            Order = 1,
            Hidden = false
        };
        var name2 = new MemberName
        {
            Id = Guid.NewGuid(),
            Member = member,
            MemberId = member.Id,
            Value = "Strider",
            Type = NameType.NickName,
            Order = 1, // Duplicate order
            Hidden = false
        };

        member.AddName(name1);

        var ex = Assert.Throws<InvalidOperationException>(() => member.AddName(name2));
        Assert.Contains("duplicate order", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}