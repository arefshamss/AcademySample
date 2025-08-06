using System.Linq.Expressions;
using Academy.Domain.Models.Logs;
using Academy.Domain.ViewModels.ApplicationLogs;

namespace Academy.Application.Mapper.ApplicationLogs;

internal static class ApplicationLogsMapper
{
    internal static Expression<Func<ApplicationLog, ApplicationLogsViewModel>> MapToApplicationLogsViewModel => item =>
        new()
        {
            Id = item.Id , 
            TimeStamp = item.TimeStamp,
            MessageTemplate = item.MessageTemplate ,
            RenderedMessage = item.RenderedMessage, 
            Level = item.Level, 
        };

    internal static ApplicationLogDetailViewModel MapToDetailViewModel(this ApplicationLog model) => new()
    {
        TimeStamp = model.TimeStamp,
        MessageTemplate = model.MessageTemplate,
        RenderedMessage = model.RenderedMessage,
        Level = model.Level,
        Exception = model.Exception , 
    };
}