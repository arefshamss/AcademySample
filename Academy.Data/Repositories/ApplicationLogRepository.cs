using System.Linq.Expressions;
using Academy.Data.Context;
using Academy.Domain.Contracts;
using Academy.Domain.Models.Logs;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Academy.Data.Repositories;

public class ApplicationLogRepository(AcademyMongoDbContext context) : IApplicationLogRepository{
    public async Task FilterAsync<TModel>(BasePaging<TModel> filterModel, FilterConditions<ApplicationLog> filterConditions, Expression<Func<ApplicationLog, TModel>> mapping,
        OrderCondition<ApplicationLog>? orderBy = null, CancellationToken cancellationToken = default)
    {
        var query = context.ApplicationLogs.AsQueryable();
     
        filterConditions?.ForEach(condition => query = query.Where(condition));
        if (orderBy is not null)
        {
            switch (orderBy.Order)
            {
                case FilterOrderBy.Ascending:
                    query = query.OrderBy(orderBy.Expression);
                    break;
                case FilterOrderBy.Descending:
                    query = query.OrderByDescending(orderBy.Expression);
                    break;
            }
        }
        else
            query = query.OrderByDescending(s => s.TimeStamp);

        await filterModel.Paging(query.AsNoTrackingWithIdentityResolution().Select(mapping),cancellationToken);
    }

    public async Task<ApplicationLog?> GetByIdAsync(ObjectId id, CancellationToken cancellationToken = default)
        => await context.ApplicationLogs.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

    public void Delete(ApplicationLog entity)
        => context.ApplicationLogs.Remove(entity);

    public void DeleteRange(List<ApplicationLog> entities)
        => context.ApplicationLogs.RemoveRange(entities);    

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await context.SaveChangesAsync(cancellationToken);

    public async Task<List<ApplicationLog>> GetAllAsync(CancellationToken cancellationToken = default)
         => await context.ApplicationLogs.ToListAsync(cancellationToken);
}
