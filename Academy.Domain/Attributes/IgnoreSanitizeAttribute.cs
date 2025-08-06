namespace Academy.Domain.Attributes;

/// <summary>
/// ignore,s sanitize for string parameters by default it,s gonna sanitize all the parameters outside the admin panel area on post actions   
/// </summary>
public class IgnoreSanitizeAttribute : Attribute;