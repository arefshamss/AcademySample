using System.Linq.Expressions;
using Academy.Domain.Models.Common;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Academy.Domain.Contracts.Generics;

public interface IEfRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Asynchronously returns the number of elements in a sequence.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the number of elements in the input sequence.
    /// </returns>
    Task<int> CountAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously returns the number of elements in a sequence.
    /// </summary>
    /// <param name="predicate">filter the result of the count based on a condition</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the number of elements in the input sequence.
    /// </returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);


    /// <summary>
    /// soft deletes an entity. the entity is never really deleted it just removed it by changing a flag 
    /// </summary>
    /// <param name="entity">the entity to soft delete</param>
    void SoftDelete(TEntity entity);

    /// <summary>
    /// changes the delete state of an entity and update it(uses soft delete)
    /// </summary>
    /// <param name="entity">entity for changing delete state</param>
    /// <returns>a boolean that that indicates the current state of soft delete if it,s true meant the current entity is deleted if it is false it means that the entity current state is changed to recovered from delete</returns>
    bool SoftDeleteOrRecover(TEntity entity);

    /// <summary>
    /// soft deletes entities based on a condition. if the condition is null soft deletes all the entities in the table.
    /// there is no need to call SaveAsync Method after for saving changes to database because This operation executes immediately against the database.  
    /// </summary>
    /// <param name="predicate">filters the entities to soft delete</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task SoftDeleteAllAsync(Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);


    /// <summary>
    /// Asynchronously returns the first element of a sequence, or a default value if the sequence contains no elements
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition</param>
    /// <param name="orderBy">order,s the entities first inside database then return,s the first element in the source based on the order condition,s.</param>
    /// <param name="includes">include properties</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains default ( TEntity ) if TEntity is empty; otherwise, the first element in source.
    /// </returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        OrderCondition<TEntity>? orderBy = null,
        CancellationToken cancellationToken = default,
        params string[] includes);

    /// <summary>
    /// Asynchronously returns the last element of a sequence, or a default value if the sequence contains no elements
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition</param>
    /// <param name="orderBy">order,s the entities first inside database then return,s the first element in the source based on the order condition,s.</param>
    /// <param name="includes">include properties</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains default ( TEntity ) if TEntity is empty; otherwise, the last element in source.
    /// </returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<TEntity?> LastOrDefaultAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        OrderCondition<TEntity>? orderBy = null,
        CancellationToken cancellationToken = default,
        params string[] includes);

    /// <summary>
    /// Begins tracking the given entity in the Deleted state such that it will be removed from the database when <see cref="SaveChangesAsync"/>> is called
    /// </summary>
    /// <param name="entity">the entity to remove</param>
    void Delete(TEntity entity);

    /// <summary>
    /// Begins tracking the given entities in the Deleted state such that it will be removed from the database when <see cref="SaveChangesAsync"/>> is called
    /// </summary>
    /// <param name="entities">a list of entities to remove</param>
    void DeleteRange(List<TEntity> entities);


    /// <summary>
    /// remove rows form the database based on a conditions. no need to call the <see cref="SaveChangesAsync"/> method this method executes automatically into database. 
    /// </summary>
    /// <param name="filter">condition to remove rows form database based on that</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    Task ExecuteDeleteRange(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);

    /// <summary>
    /// remove rows form the database based on a conditions. no need to call the <see cref="SaveChangesAsync"/> method this method executes automatically into database. 
    /// </summary>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task ExecuteDeleteRange(CancellationToken cancellationToken = default);


    /// <summary>
    ///   updates a property against all the entities matches the predicate filter parameter. no need to call the <see cref="SaveChangesAsync"/> method this method executes automatically into the database. 
    /// </summary>
    /// <param name="predicate">filter,s the result based on the condition</param>
    /// <param name="setPropertyCalls">set the property value of the entities </param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task ExecuteUpdateAsync(Expression<Func<TEntity, bool>> predicate,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
        CancellationToken cancellationToken = default);


    /// <summary>
    ///   updates a property against all the entities matches the predicate filter parameter. no need to call the <see cref="SaveChangesAsync"/> method this method executes automatically into the database. 
    /// </summary>
    /// <param name="setPropertyCalls">set the property value of the entities </param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task ExecuteUpdateAsync(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setPropertyCalls,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///  gets the list of all entities inside a table from the database
    /// </summary>
    /// <param name="includes">A list of include properties function,s to join the relational properties into the <see cref="TEntity"/> </param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>a list of all entities within the table inside database</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default , params string[] includes);

    /// <summary>
    ///  gets the list of all entities inside a table from the database
    /// </summary>
    /// <param name="filter">filter,s the result based on the given condition</param>
    /// <param name="includes">include properties</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>a list of all entities within the table inside database</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default ,
        params string[] includes);

    /// <summary>
    ///  gets the list of all entities inside a table from the database
    /// </summary>
    /// <param name="mapping">a function that maps the final result into the given model</param>
    /// <param name="filter">filter,s the result based on the given condition</param>
    /// <param name="includes">include properties</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>a list of <see cref="TModel"/>  that is mapped from all entities within the database</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<List<TModel>> GetAllAsync<TModel>(Expression<Func<TEntity, TModel>> mapping,
        Expression<Func<TEntity, bool>> filter,
        CancellationToken cancellationToken = default , 
        params string[] includes);



    /// <summary>
    /// gets the list of entities using pagination. can apply filtering and ordering too. 
    /// </summary>
    /// <param name="filterModel">the paging filter model</param>
    /// <param name="filterConditions">conditions to filter the result before paging</param>
    /// <param name="mapping">a function that map,s the entities into the selected result type,s list</param>
    /// <param name="includes">include properties</param>
    /// <param name="orderBy">orders the result based on the <see cref="FilterOrderBy"/> condition,s</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <typeparam name="TModel">the model that filter maps the result into</typeparam>
    /// <returns>return,s a <see cref="BasePaging{T}"/> model with a list of <see cref="TModel"/> as the Entities Property </returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task FilterAsync<TModel>(BasePaging<TModel> filterModel, FilterConditions<TEntity> filterConditions,
        Expression<Func<TEntity, TModel>> mapping,
        OrderCondition<TEntity>? orderBy = null,
        CancellationToken cancellationToken = default,
        params string[] includes);

    /// <summary>
    /// Asynchronously invokes a projection function on each element of a sequence and returns the maximum resulting value.
    /// </summary>
    /// <param name="filterConditions">filter,s the result before returning the maximum resulting value</param>
    /// <param name="select">A projection function to apply to each element.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResult">The type of the value returned by the function represented by selector.</typeparam>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the maximum value in the sequence or the default value if any exception,s happen
    /// </returns>
    Task<TResult> MaxAsync<TResult>(FilterConditions<TEntity> filterConditions,
        Expression<Func<TEntity, TResult>> select
        , CancellationToken cancellationToken = default);

    
    /// <summary>
    /// Asynchronously invokes a projection function on each element of a sequence and returns the minimum resulting value.
    /// </summary>
    /// <param name="filterConditions">filter,s the result before returning the minimum resulting value</param>
    /// <param name="select">A projection function to apply to each element.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <typeparam name="TResult">The type of the value returned by the function represented by selector.</typeparam>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the minimum value in the sequence or the default value if any exception,s happen
    /// </returns>
    Task<TResult> MinAsync<TResult>(FilterConditions<TEntity> filterConditions,
        Expression<Func<TEntity, TResult>> select, CancellationToken cancellationToken = default);


    /// <summary>
    /// Finds an entity with the given primary key values. if no entity found returns null
    /// </summary>
    /// <param name="id">primary key of the entity</param>
    /// <param name="includes">include properties</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>founded entity or null if no entity found</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<TEntity?> GetByIdAsync(TKey id,
        CancellationToken cancellationToken = default , params string[] includes);

    /// <summary>
    /// Begins tracking the given entity, and any other reachable entities that are not already being tracked, in the Added state such that they will be inserted into the database when <see cref="SaveChangesAsync"/> is called
    /// </summary>
    /// <param name="entity">The entity to add</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins tracking the given entities, and any other reachable entities that are not already being tracked, in the Added state such that they will be inserted into the database when <see cref="SaveChangesAsync"/> is called
    /// </summary>
    /// <param name="entities">The entities to add</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task InsertRangeAsync(List<TEntity> entities, CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins tracking the given entity and entries reachable from the given entity using the Modified state by default
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    void Update(TEntity entity);


    /// <summary>
    /// Begins tracking the given entity and entities reachable from the given entity using the Modified state by default
    /// </summary>
    /// <param name="entities">The entities to update.</param>
    void UpdateRange(List<TEntity> entities);

    /// <summary>
    /// Asynchronously determines whether any element of a sequence satisfies a condition.
    /// </summary>
    /// <param name="predicate">A function to test each element for a condition</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains true if any elements in the source sequence pass the test in the specified predicate; otherwise, false.
    /// </returns>
    /// <exception cref="ArgumentNullException">source or predicate is null.</exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <exception cref="DbUpdateException">An error is encountered while saving to the database.</exception>
    /// <exception cref="DbUpdateConcurrencyException">
    /// A concurrency violation is encountered while saving to the database. A concurrency violation occurs when an unexpected number of rows are affected during save. This is usually because the data in the database has been modified since it was loaded into memory.
    /// </exception>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);

    
    /// <summary>
    /// Asynchronously computes the sum of the sequence of values that is obtained by invoking a projection function on each element of the input sequence.
    /// </summary>
    /// <param name="selector">selected property for sum</param>
    /// <param name="predicate">Filters the result based on a predicate.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the sum of the projected values..
    /// </returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<int> SumAsync(Expression<Func<TEntity, int>> selector, Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously computes the sum of the sequence of values that is obtained by invoking a projection function on each element of the input sequence.
    /// </summary>
    /// <param name="selector">selected property for sum</param>
    /// <param name="conditions">a list of conditions to filter the result based on that</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the sum of the projected values..
    /// </returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<int> SumAsync(Expression<Func<TEntity, int>> selector, FilterConditions<TEntity> conditions,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously computes the sum of the sequence of values that is obtained by invoking a projection function on each element of the input sequence.
    /// </summary>
    /// <param name="selector">selected property for sum</param>
    /// <param name="predicate">Filters the result based on a predicate.</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the sum of the projected values..
    /// </returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<double> SumAsync(Expression<Func<TEntity, double>> selector, Expression<Func<TEntity, bool>>? predicate = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously computes the sum of the sequence of values that is obtained by invoking a projection function on each element of the input sequence.
    /// </summary>
    /// <param name="selector">selected property for sum</param>
    /// <param name="conditions">a list of conditions to filter the result based on that</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the sum of the projected values..
    /// </returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<double> SumAsync(Expression<Func<TEntity, double>> selector, FilterConditions<TEntity> conditions,
        CancellationToken cancellationToken = default);

}

public interface IEfRepository<TEntity> : IEfRepository<TEntity, int>
    where TEntity : BaseEntity<int>;