namespace FamilyArchive.Domain.Enums;

public enum PartnershipType
{
    [DisplayName("Marriage")]
    Marriage,
    
    [DisplayName("Civil Union")]
    CivilUnion,
    
    [DisplayName("Domestic Partnership")]
    DomesticPartnership,
    
    [DisplayName("Common Law Partnership")]
    CommonLawPartnership,
    
    [DisplayName("Engagement")]
    Engagement,
    
    [DisplayName("Cohabitation")]
    Cohabitation,
    
    [DisplayName("Registration Partnership")]
    RegistrationPartnership,
    
    [DisplayName("Other")]
    Other,
    
    [DisplayName("Unknown")]
    Unknown
}
public static class PartnershipTypeExtensions
{
    public static string GetDisplayName(this PartnershipType type)
    {
        var field = type.GetType().GetField(type.ToString());
        var attribute = field?.GetCustomAttributes(typeof(DisplayNameAttribute), false)
            .FirstOrDefault() as DisplayNameAttribute;
        return attribute?.DisplayName ?? type.ToString();
    }
}