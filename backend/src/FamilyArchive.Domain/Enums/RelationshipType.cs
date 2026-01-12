using System;

namespace FamilyArchive.Domain.Enums;

public enum RelationshipType
{
    [DisplayName("Biological Mother")]
    BiologicalMother,

    [DisplayName("Biological Father")]
    BiologicalFather,

    [DisplayName("Stepmother")]
    StepMother,

    [DisplayName("Stepfather")]
    StepFather,

    [DisplayName("Adoptive Mother")]
    AdoptiveMother,

    [DisplayName("Adoptive Father")]
    AdoptiveFather,

    //typically used in polyamorous or non-traditional family structures
    [DisplayName("Other Mother")]
    OtherMother,

    [DisplayName("Other Father")]
    OtherFather,

    [DisplayName("Guardian")]
    Guardian,

    [DisplayName("Other")]
    Other
}

public static class RelationshipTypeExtensions
{
    public static string GetDisplayName(this RelationshipType type)
    {
        var field = type.GetType().GetField(type.ToString());
        var attribute = field?.GetCustomAttributes(typeof(DisplayNameAttribute), false)
            .FirstOrDefault() as DisplayNameAttribute;
        return attribute?.DisplayName ?? type.ToString();
    }
}