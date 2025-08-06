using System.Text.RegularExpressions;
using Academy.Domain.Shared;

namespace Academy.Application.Tools;

public partial class UserRegex
{
    [GeneratedRegex(SiteRegex.MobileRegex)]
    public static partial Regex MobileRegex();

    
    [GeneratedRegex(SiteRegex.EmailRegex)]
    public static partial Regex EmailRegex();
    
    
    [GeneratedRegex(SiteRegex.NationalCodeRegex)]
    public static partial Regex NationalCodeRegex();   
    
    
    [GeneratedRegex(SiteRegex.ConfirmationCodeRegex)]
    public static partial Regex ConfirmationCodeRegex();
}