using System.Security.Claims;
using System.Security.Principal;

namespace Academy.Application.Extensions;

public static class IdentityExtensions
{
    #region UserId

    public static UserId GetUserId(this IPrincipal principal)
    {
        if (principal == null)
            return default;

        var user = (ClaimsPrincipal)principal;
        return user.GetUserId();
    }
    
    public static UserId GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal == null)
            return default;

        if (claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier) == null)
            return default;

        string userId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier).Value;
        if (string.IsNullOrEmpty(userId))
            return default;

        bool isParseSuccess = int.TryParse(userId, out int parsedUserId);
        if (!isParseSuccess) return default;

        return parsedUserId;
    }


    // public static int? GetUserId(this IPrincipal principal)
    // {
    //     if (principal == null)
    //         return default;
    //
    //     var user = (ClaimsPrincipal)principal;
    //
    //     return user.GetUserId();
    // }

    #endregion
    
    #region UserName

    public static string? GetUserName(this ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal == null)
            return default;

        if (claimsPrincipal.FindFirst(ClaimTypes.Name) == null)
            return default;

        string userName = claimsPrincipal.FindFirst(ClaimTypes.Name).Value;
        if (string.IsNullOrEmpty(userName))
            return default;

        return userName;
    }

    public static string? GetUserName(this IPrincipal principal)
    {
        if (principal == null)
            return default;

        var user = (ClaimsPrincipal)principal;

        return user.GetUserName();
    }

    #endregion
}

public struct UserId(int? value) : IEquatable<UserId>
{
    private readonly int? _value = value;

    // Implicit conversion from UserId to int?
    
    public static implicit operator int(UserId userId)
    {
        return userId._value ?? default(int);
    }
    
    public static implicit operator int?(UserId userId)
    {
        return userId._value;
    }

    // Implicit conversion from int? to UserId
    public static implicit operator UserId(int? value)
    {
        return new UserId(value);
    }

    // Override ToString for better usability
    public override string ToString()
    {
        return _value?.ToString() ?? "";
    }

    public bool Equals(UserId other)
    {
        return _value == other._value;
    }

    public override bool Equals(object? obj)
    {
        return obj is UserId other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }
}
