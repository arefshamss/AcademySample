using System.Linq.Expressions;
using Academy.Data.Context;
using Academy.Domain.Contracts.Generics;
using Academy.Domain.Models.Common;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;

namespace Academy.Data.Repositories.Generics;

public class EfRepository<TEntity, TKey> : IEfRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly AcademyContext Context;
    protected readonly DbSet<TEntity> DbSet;

    protected EfRepository(AcademyContext context)
    {
        Context = context;
        DbSet = Context.Set<TEntity>();
    }


    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        => await Context.Database.BeginTransactionAsync(cancellationToken);


    public async Task<int> CountAsync(CancellationToken cancellationToken = default)
        => await DbSet.CountAsync(cancellationToken);

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await DbSet.CountAsync(predicate, cancellationToken);


    public void SoftDelete(TEntity entity)
    {
        entity.IsDeleted = true;
        entity.DeletedDate = DateTime.Now;
        DbSet.Update(entity);
    }

    public bool SoftDeleteOrRecover(TEntity entity)
    {
        entity.IsDeleted = !entity.IsDeleted;
        if (entity.IsDeleted) entity.DeletedDate = DateTime.Now;
        else entity.DeletedDate = null;
        DbSet.Update(entity);
        return entity.IsDeleted;
    }

    public async Task SoftDeleteAllAsync(Expression<Func<TEntity, bool>>? filter = null,
        CancellationToken cancellationToken = default)
    {
        if (filter is null)
            await DbSet.ExecuteUpdateAsync(setters => setters.SetProperty(s => s.IsDeleted, true), cancellationToken);
        else
            await DbSet.Where(filter)
                .ExecuteUpdateAsync(setters => setters.SetProperty(s => s.IsDeleted, true), cancellationToken);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
        OrderCondition<TEntity>? orderBy = null,
        CancellationToken cancellationToken = default, params string[] includes)
    {
        var query = DbSet.AsQueryable();
        if (predicate is not null)
            query = query.Where(predicate);

        includes?.ToList().ForEach(include => query = query.Include(include));

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

        return await query
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TEntity?> LastOrDefaultAsync(Expression<Func<TEntity, bool>>? predicate = null,
        OrderCondition<TEntity>? orderBy = null,
        CancellationToken cancellationToken = default, params string[] includes)
    {
        var query = DbSet.AsQueryable();
        if (predicate is not null)
            query = query.Where(predicate);


        includes?.ToList().ForEach(include => query = query.Include(include));


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

        return await query.LastOrDefaultAsync(cancellationToken);
    }

    public void Delete(TEntity entity) => DbSet.Remove(entity);

    public void DeleteRange(List<TEntity> entities) => DbSet.RemoveRange(entities);

    public async Task ExecuteDeleteRange(Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default)
        => await DbSet.Where(filter).ExecuteDeleteAsync(cancellationToken);


    public async Task ExecuteDeleteRange(CancellationToken cancellationToken = default)
        => await DbSet.ExecuteDeleteAsync(cancellationToken);

    public async Task ExecuteUpdateAsync(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();
        query = query.Where(predicate);

        await query.ExecuteUpdateAsync(setPropertyCalls, cancellationToken);
    }

    public async Task ExecuteUpdateAsync(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();

        await query.ExecuteUpdateAsync(setPropertyCalls, cancellationToken);
    }

    public Task<List<TEntity>> GetAllAsync(
        CancellationToken cancellationToken = default,
        params string[] includes)
    {
        var query = DbSet.AsQueryable();
        includes?.ToList().ForEach(include => query = query.Include(include));
        return query.AsNoTrackingWithIdentityResolution().ToListAsync(cancellationToken);
    }

    public Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default, params string[] includes)
    {
        var query = DbSet.AsQueryable();
        query = query.Where(filter);
        includes?.ToList().ForEach(include => query = query.Include(include));
        return query.AsNoTrackingWithIdentityResolution().ToListAsync(cancellationToken);
    }

    public Task<List<TModel>> GetAllAsync<TModel>(Expression<Func<TEntity, TModel>> mapping,
        Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default,
        params string[] includes)
    {
        var query = DbSet.AsQueryable();
        query = query.Where(filter);
        includes?.ToList().ForEach(include => query = query.Include(include));
        return query.AsNoTrackingWithIdentityResolution().Select(mapping).ToListAsync(cancellationToken);
    }

    public async Task FilterAsync<TModel>(BasePaging<TModel> filterModel, FilterConditions<TEntity> filterConditions,
        Expression<Func<TEntity, TModel>> mapping,
        OrderCondition<TEntity>? orderBy = null,
        CancellationToken cancellationToken = default,
        params string[] includes)
    {
        IQueryable<TEntity> query = DbSet;
        includes?.ToList().ForEach(include => query = query.Include(include));
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
            query = query.OrderByDescending(s => s.CreatedDate);

        await filterModel.Paging(query.AsNoTrackingWithIdentityResolution().Select(mapping), cancellationToken);
    }

    public async Task FilterAsync<TModel, TGroupKey>(GroupedPaging<TGroupKey, TModel> filterModel, FilterConditions<TEntity> filterConditions,
        Expression<Func<TEntity, TModel>> mapping,
        Expression<Func<TEntity, TGroupKey>> groupBy,
        OrderCondition<TEntity>? orderBy = null,
        CancellationToken cancellationToken = default,
        params string[] includes)
    {
        IQueryable<TEntity> query = DbSet;
        includes?.ToList().ForEach(include => query = query.Include(include));
        filterConditions?.ForEach(condition => query = query.Where(condition));
        var groupedQuery = query.GroupBy(groupBy);
        if (orderBy is not null)
        {
            switch (orderBy.Order)
            {
                case FilterOrderBy.Ascending:
                    groupedQuery = groupedQuery.OrderBy(s => s.Key);
                    break;
                case FilterOrderBy.Descending:
                    groupedQuery = groupedQuery.OrderByDescending(s => s.Key);
                    break;
            }
        }
        else
            groupedQuery = groupedQuery.OrderByDescending(s => s.Key);

        var result = groupedQuery
            .AsNoTrackingWithIdentityResolution()
            .Select(s => new GroupedResult<TGroupKey, TModel>
            {
                Key = s.Key,
                Items = s.Select(a => mapping.Compile()(a)).ToList()
            });

        await filterModel.Paging(result.AsQueryable(), cancellationToken);
    }

    public async Task FilAsync<TModel>(BasePaging<TModel> filterModel, FilterConditions<TEntity> filterConditions,
        Expression<Func<TEntity, TModel>> mapping,
        OrderConditions<TEntity>? orderConditions = null,
        CancellationToken cancellationToken = default,
        params string[] includes)
    {
        IQueryable<TEntity> query = DbSet;
        includes?.ToList().ForEach(include => query = query.Include(include));
        filterConditions?.ForEach(condition => query = query.Where(condition));

        if (orderConditions != null && orderConditions.Count > 0)
        {
            IOrderedQueryable<TEntity> orderedQuery = orderConditions[0].Order == FilterOrderBy.Ascending
                ? query.OrderBy(orderConditions[0].Expression)
                : query.OrderByDescending(orderConditions[0].Expression);

            for (int i = 1; i < orderConditions.Count; i++)
            {
                var orderCondition = orderConditions[i];
                orderedQuery = orderCondition.Order == FilterOrderBy.Ascending
                    ? orderedQuery.ThenBy(orderCondition.Expression)
                    : orderedQuery.ThenByDescending(orderCondition.Expression);
            }

            query = orderedQuery;
        }
        else
        {
            query = query.OrderByDescending(s => s.CreatedDate);
        }

        await filterModel.Paging(query.AsNoTrackingWithIdentityResolution().Select(mapping), cancellationToken);
    }

    public async Task<TResult> MaxAsync<TResult>(FilterConditions<TEntity> filterConditions,
        Expression<Func<TEntity, TResult>> select,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTrackingWithIdentityResolution().AsQueryable();
        filterConditions?.ForEach(condition => query = query.Where(condition));
        try
        {
            return await query.MaxAsync(select, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return default;
        }
        catch (InvalidOperationException)
        {
            return default;
        }
    }

    public async Task<TResult> MinAsync<TResult>(FilterConditions<TEntity> filterConditions,
        Expression<Func<TEntity, TResult>> select,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTrackingWithIdentityResolution().AsQueryable();
        filterConditions?.ForEach(condition => query = query.Where(condition));
        try
        {
            return await query.MinAsync(select, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            return default;
        }
        catch (InvalidOperationException)
        {
            return default;
        }
    }

    public async Task<TEntity?> GetByIdAsync(TKey id,
        CancellationToken cancellationToken = default, params string[] includes)
    {
        if (id is null) return null;
        var query = DbSet.AsQueryable();
        includes?.ToList().ForEach(include => query = query.Include(include));
        return await query.FirstOrDefaultAsync(entity => entity.Id.Equals(id), cancellationToken);
    }

    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        => await DbSet.AddAsync(entity, cancellationToken);

    public async Task InsertRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default)
        => await DbSet.AddRangeAsync(entities, cancellationToken);

    public void Update(TEntity entity) => DbSet.UpdateRange(entity);

    public void UpdateRange(List<TEntity> entities) => DbSet.UpdateRange(entities);

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken = default)
        => await DbSet.AnyAsync(predicate, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await Context.SaveChangesAsync(cancellationToken);

    public async Task<int> SumAsync(Expression<Func<TEntity, int>> selector,
        Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();

        if (predicate != null)
            query = query.Where(predicate);

        return await query.SumAsync(selector, cancellationToken);
    }

    public async Task<int> SumAsync(Expression<Func<TEntity, int>> selector, FilterConditions<TEntity> conditions,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();

        conditions.ForEach(expression => query = query.Where(expression));

        return await query.SumAsync(selector, cancellationToken);
    }

    public async Task<double> SumAsync(Expression<Func<TEntity, double>> selector,
        Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();

        if (predicate != null)
            query = query.Where(predicate);

        return await query.SumAsync(selector, cancellationToken);
    }

    public async Task<double> SumAsync(Expression<Func<TEntity, double>> selector, FilterConditions<TEntity> conditions,
        CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsQueryable();

        conditions.ForEach(expression => query = query.Where(expression));

        return await query.SumAsync(selector, cancellationToken);
    }
}

public class EfRepository<TEntity>(AcademyContext context)
    : EfRepository<TEntity, int>(context), IEfRepository<TEntity>
    where TEntity : BaseEntity<int>;