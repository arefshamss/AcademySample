using System.Linq.Expressions;
using Academy.Domain.ViewModels.UserActivity;

namespace Academy.Application.Mapper.UserActivity;

public static class UserActivityMapper
{

    public static Expression<Func<Academy.Domain.Models.UserActivities.UserActivity, UserActivityViewModel>>
        MapToUserActivityViewModel() =>
        userActivity => new()
        {
            Id = userActivity.Id , 
            Description = userActivity.Description , 
            BrowserName = userActivity.BrowserName , 
            BrowserVersion = userActivity.BrowserVersion , 
            IpAddress = userActivity.IpAddress , 
            StatusCode = userActivity.StatusCode , 
            CreatedDate = userActivity.CreatedDate
        };

    public static UserActivityDetailViewModel MapToUserActivityDetailViewModel(
        this Academy.Domain.Models.UserActivities.UserActivity userActivity) => new()
    {
        CreatedDate = userActivity.CreatedDate.ToLongDateString() ,
        Brand = userActivity.Brand , 
        Model = userActivity.Model , 
        Description = userActivity.Description , 
        Device = userActivity.Device , 
        Os = userActivity.Os , 
        BrowserName = userActivity.BrowserName , 
        BrowserVersion = userActivity.BrowserVersion , 
        IpAddress = userActivity.IpAddress , 
        OsPlatform = userActivity.OsPlatform , 
        OsVersion = userActivity.OsVersion , 
        IsBot = userActivity.IsBot , 
        BotCategory = userActivity.BotCategory , 
        BotName = userActivity.BotName , 
        BotProducer = userActivity.BotProducer, 
        BotUrl = userActivity.BotUrl , 
        UserId = userActivity.UserId , 
        Area = userActivity.Area , 
        Url = userActivity.Url  , 
        FormData = userActivity.FormData , 
        StatusCode = userActivity.StatusCode , 
        
    };
    
    public static Academy.Domain.Models.UserActivities.UserActivity MapToUserActivity(this InsertUserActivityViewModel model) =>
        new()
        {
            Brand = model.Brand , 
            Model = model.Model , 
            Description = model.Description , 
            Device = model.Device ,
            Os = model.Os , 
            BrowserName = model.BrowserName  ,
            BrowserVersion = model.BrowserVersion , 
            IpAddress = model.IpAddress , 
            OsPlatform = model.OsPlatform , 
            OsVersion = model.OsVersion , 
            IsBot = model.IsBot , 
            BotCategory = model.BotCategory , 
            BotName = model.BotName     , 
            BotProducer = model.BotProducer , 
            BotUrl = model.BotUrl ,
            UserId = model.UserId , 
            Area = model.Area ,
            Url = model.Url , 
            FormData = model.FormData , 
            StatusCode = model.StatusCode 
        };
    
}