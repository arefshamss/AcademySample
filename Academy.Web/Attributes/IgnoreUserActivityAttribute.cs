namespace Academy.Web.Attributes;


/// <summary>
/// used to make the action without the need to use <see cref="UserActivityLogAttribute"/> and ignoring the activity log    
/// </summary>
public sealed class IgnoreUserActivityAttribute: Attribute{}