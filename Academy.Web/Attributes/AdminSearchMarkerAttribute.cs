namespace Academy.Web.Attributes;

/// <summary>
/// used for searches inside the admin panel of site. takes an title for search title of an link
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false)]
public sealed class AdminSearchMarkerAttribute : Attribute
{
    public required string Title {
        get;
        set;
    }
}