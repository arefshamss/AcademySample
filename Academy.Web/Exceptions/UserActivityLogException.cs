namespace Academy.Web.Exceptions;

public class UserActivityLogException : Exception
{
    public UserActivityLogException(string action, string controller, string area) : 
        base(
        $"UserActivityFilterAttribute must be defined at the action level. please eighter provide the attribute  at the action level so the user activity logs can work properly or user IgnoreUserActivityAttribute. Action: {action} , Controller : {controller}  , Area : {area} ")
    {
    }
}