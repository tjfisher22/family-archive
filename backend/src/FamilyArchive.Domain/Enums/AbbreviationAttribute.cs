namespace FamilyArchive.Domain.Enums;

[AttributeUsage(AttributeTargets.Field)]
public class AbbreviationAttribute : Attribute
{
    public char Abbreviation { get; }

    public AbbreviationAttribute(char abbreviation)
    {
        Abbreviation = abbreviation;
    }
}
