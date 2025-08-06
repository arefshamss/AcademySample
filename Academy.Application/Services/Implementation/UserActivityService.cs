using Academy.Application.Mapper.UserActivity;
using Academy.Application.Services.Interfaces;
using Academy.Application.Extensions;
using Academy.Domain.Contracts;
using Academy.Domain.Models.UserActivities;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.UserActivity;
using MongoDB.Bson;

namespace Academy.Application.Services.Implementation;

public class UserActivityService(IUserActivityRepository userActivityRepository) : IUserActivityService
{
    public async Task InsertAsync(InsertUserActivityViewModel model)
    {
        await userActivityRepository.InsertAsync(model.MapToUserActivity());
        await userActivityRepository.SaveChangesAsync();
    }

    public async Task<Result<FilterUserActivityViewModel>> FilterAsync(
        FilterUserActivityViewModel filter)
    {
        filter ??= new();
        var conditions = Filter.GenerateConditions<UserActivity>();
        var orderCondition =Filter.GenerateOrder<UserActivity>(ua=>ua.CreatedDate, FilterOrderBy.Descending);
    
        #region Filters

        if (!filter.Description.IsNullOrEmptyOrWhiteSpace())
            conditions.Add(s => s.Description.Contains(filter.Description!));

        if (!filter.IpAddress.IsNullOrEmptyOrWhiteSpace())
            conditions.Add(s => s.IpAddress.Contains(filter.IpAddress!));

        if (!filter.BrowserName.IsNullOrEmptyOrWhiteSpace())
            conditions.Add(s => s.BrowserName.Contains(filter.BrowserName!));

        if (!filter.Url.IsNullOrEmptyOrWhiteSpace())
            conditions.Add(s => s.Url.Contains(filter.Url!));

        if(!filter.FromDate.IsNullOrEmptyOrWhiteSpace())
            conditions.Add(s  => s.CreatedDate >= filter.FromDate!.ToMiladiDateTime());
        
        if(!filter.ToDate.IsNullOrEmptyOrWhiteSpace())
            conditions.Add(s => s.CreatedDate <= filter.ToDate!.ToMiladiDateTime());

        #endregion
        
        await userActivityRepository.FilterAsync(filter, conditions, UserActivityMapper.MapToUserActivityViewModel(), orderCondition);
        return filter;
    }

    public async Task<Result<UserActivityDetailViewModel>> GetDetailAsync(ObjectId id)
    {
        if (id == null)
            return Result.Failure<UserActivityDetailViewModel>(ErrorMessages.BadRequestError);

        var userActivity = await userActivityRepository.GetByIdAsync(id);

        if (userActivity is null)
            return Result.Failure<UserActivityDetailViewModel>(ErrorMessages.BadRequestError);

        return userActivity.MapToUserActivityDetailViewModel();
    }
    
}