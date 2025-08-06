using Academy.Application.Mapper.ApplicationLogs;
using Academy.Application.Services.Interfaces;
using Academy.Application.Extensions;
using Academy.Domain.Contracts;
using Academy.Domain.Enums.ApplicationLogs;
using Academy.Domain.Models.Logs;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.ApplicationLogs;
using MongoDB.Bson;

namespace Academy.Application.Services.Implementation;

public class ApplicationLogService(IApplicationLogRepository applicationLogRepository): IApplicationLogService
{
    public async Task<Result<FilterApplicationLogsViewModel>> FilterAsync(FilterApplicationLogsViewModel filter)
    {
        filter ??= new ();

        var conditions = Filter.GenerateConditions<ApplicationLog>();
        
        var fromDate = filter.FromDate?.ToMiladiDateTime();
        var toDate = filter.ToDate?.ToMiladiDateTime();

        if (fromDate != null && toDate != null && (fromDate > toDate))
            return Result.Failure<FilterApplicationLogsViewModel>(ErrorMessages.StartDateBiggerThanEndDateError);

        if (fromDate.HasValue)
            conditions.Add(s => s.TimeStamp >= fromDate.Value);

        if (toDate.HasValue)
            conditions.Add(s => s.TimeStamp <= toDate.Value);

        
        if(filter.Level != FilterLogLevel.All)
            conditions.Add(s =>  s.Level == filter.Level.ToString());


        await applicationLogRepository.FilterAsync(filter, conditions,
            ApplicationLogsMapper.MapToApplicationLogsViewModel);

        return filter;
    }

    public async Task<Result> DeleteAsync(ObjectId id)
    {
        var applicationLog = await applicationLogRepository.GetByIdAsync(id);

        if (applicationLog is null) return Result.Failure(ErrorMessages.BadRequestError);
        
        applicationLogRepository.Delete(applicationLog);
        await applicationLogRepository.SaveChangesAsync();
        return Result.Success(SuccessMessages.DeleteSuccess);
    }
    

    public async Task DeleteAllAsync()
    {
        var logs = await applicationLogRepository.GetAllAsync();
        applicationLogRepository.DeleteRange(logs);
        await applicationLogRepository.SaveChangesAsync();
    }

    public async Task<Result<ApplicationLogDetailViewModel>> GetDetailAsync(ObjectId id)
    {
        var applicationLog = await applicationLogRepository.GetByIdAsync(id);
        if(applicationLog is null)
            return Result.Failure<ApplicationLogDetailViewModel>(ErrorMessages.BadRequestError);

        return applicationLog.MapToDetailViewModel();
    }
}