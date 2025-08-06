namespace Academy.Domain.Shared;

public static class SiteRegex
{
    public const string MobileRegex = @"^(98|0)?(۹۸|۰)?(9|۹)[۰-۹0-9]{9}?$";


    public const string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    
    
    public const string NationalCodeRegex = @"^\d{10}$";
    
    
    public const string ConfirmationCodeRegex = @"^[۰۱۲۳۴۵۶۷۸۹0-9]+$";

}