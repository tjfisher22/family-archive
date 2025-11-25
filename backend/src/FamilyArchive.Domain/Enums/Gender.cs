namespace FamilyArchive.Domain.Enums;

public enum Gender
{
    [DisplayName("Male")]
    [Abbreviation('M')]
    Male,
    
    [DisplayName("Female")]
    [Abbreviation('F')]
    Female,
    
    [DisplayName("Non-Binary")]
    [Abbreviation('X')]
    NonBinary,
    
    [DisplayName("Unspecified")]
    [Abbreviation('?')]
    Unspecified,
    
    [DisplayName("Other")]
    [Abbreviation('O')]
    Other
}

public static class GenderExtensions
{
    public static string GetDisplayName(this Gender gender)
    {
        var field = gender.GetType().GetField(gender.ToString());
        var attribute = field?.GetCustomAttributes(typeof(DisplayNameAttribute), false)
            .FirstOrDefault() as DisplayNameAttribute;
        return attribute?.DisplayName ?? gender.ToString();
    }

    /// <summary>
    /// Gets the single-character abbreviation for the gender.
    /// Useful for building gender sequences in relationship paths (e.g., "MFM").
    /// </summary>
    public static char GetAbbreviation(this Gender gender)
    {
        var field = gender.GetType().GetField(gender.ToString());
        var attribute = field?.GetCustomAttributes(typeof(AbbreviationAttribute), false)
            .FirstOrDefault() as AbbreviationAttribute;
        return attribute?.Abbreviation ?? '?';
    }
}