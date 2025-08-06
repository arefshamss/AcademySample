using Academy.Domain.Shared;
using Academy.Domain.ViewModels.UserActivity;
using MongoDB.Bson;

namespace Academy.Application.Services.Interfaces;

public interface IUserActivityService
{
    Task InsertAsync(InsertUserActivityViewModel model);

    Task<Result<FilterUserActivityViewModel>> FilterAsync(FilterUserActivityViewModel filter);

    Task<Result<UserActivityDetailViewModel>> GetDetailAsync(ObjectId id);
    
}