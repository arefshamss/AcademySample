using Academy.Domain.Models.User;

namespace Academy.Domain.Extensions;

public static class UserExtensions
{
    public static string GetUserDisplayName(this User user)
    {
        if (user is { FirstName: not null, LastName: not null })
            return $"{user.FirstName} {user.LastName}";

        return user.Mobile ?? "کاربر نامشخص";
        return "کاربر نامشخص";
    }

    public static string GetUserFullName(this User user)
        => $"{user.FirstName} {user.LastName}";

}