using System.Linq.Expressions;

namespace Academy.Domain.Shared;




public class FilterConditions<TEntity> : List<Expression<Func<TEntity, bool>>>;

public static class Filter
{
    public static Expression<Func<T, bool>> CombinePredicates<T>(
        IEnumerable<Expression<Func<T, bool>>> predicates,
        Func<Expression, Expression, BinaryExpression> mergeLogic)
    {
        if (!predicates.Any())
            return _ => true; // Default to true if no predicates

        var parameter = Expression.Parameter(typeof(T));
        Expression body = predicates
            .Select(predicate => Expression.Invoke(predicate, parameter))
            .Aggregate(mergeLogic);

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
    
    public static OrderCondition<TEntity> GenerateOrder<TEntity>(Expression<Func<TEntity, object>> expression,
        FilterOrderBy orderBy = FilterOrderBy.Ascending) => new(expression, orderBy);


    public static FilterConditions<TEntity> GenerateConditions<TEntity>() => [];

    public static OrderConditions<TEntity> GenerateOrders<TEntity>() => [];
}


public class OrderCondition<TEntity>(
    Expression<Func<TEntity, object>> expression,
    FilterOrderBy orderBy = FilterOrderBy.Ascending)
{
    public FilterOrderBy Order { get; set; } = orderBy;

    public Expression<Func<TEntity, object>> Expression { get; private set; } = expression;
}

public class OrderConditions<TEntity> : List<OrderCondition<TEntity>>
{
    public void Add(Expression<Func<TEntity, object>> expression,
        FilterOrderBy orderBy = FilterOrderBy.Ascending)
        => this.Add(new OrderCondition<TEntity>(expression, orderBy));
}



public enum FilterOrderBy
{
    Ascending,
    Descending
}