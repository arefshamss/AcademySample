using Academy.Domain.Shared;
using Academy.Domain.ViewModels.ApplicationLogs;
using MongoDB.Bson;

namespace Academy.Application.Services.Interfaces;

public interface IApplicationLogService
{
    Task<Result<FilterApplicationLogsViewModel>> FilterAsync(FilterApplicationLogsViewModel filter);

    Task<Result> DeleteAsync(ObjectId id);

    Task DeleteAllAsync();
    
    Task<Result<ApplicationLogDetailViewModel>> GetDetailAsync(ObjectId id);
}