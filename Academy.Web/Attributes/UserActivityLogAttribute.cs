namespace Academy.Web.Attributes;


/// <summary>
/// used to save user activities inside the application. takes an Description attribute for saving the log description
/// </summary>
public sealed class UserActivityLogAttribute : Attribute
{
    public required string Description { get; set; }
}