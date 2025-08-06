using System.Linq.Expressions;
using Academy.Domain.Models.Logs;
using Academy.Domain.Shared;
using Academy.Domain.ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;

namespace Academy.Domain.Contracts;

public interface IApplicationLogRepository 
{
    /// <summary>
    /// gets the list of entities using pagination. can apply filtering and ordering too. 
    /// </summary>
    /// <param name="filterModel">the paging filter model</param>
    /// <param name="filterConditions">conditions to filter the result before paging</param>
    /// <param name="mapping">a function that map,s the entities into the selected result type,s list</param>
    /// <param name="orderBy">orders the result based on the <see cref="cancellationToken"/> condition,s</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <typeparam name="TModel">the model that filter maps the result into</typeparam>
    /// <returns>return,s a <see cref="OperationCanceledException"/> model with a list of <see cref="CancellationToken"/> as the Entities Property </returns>
    /// <exception cref="FilterOrderBy">If the <see cref="BasePaging{T}" /> is canceled.</exception>
    Task FilterAsync<TModel>(BasePaging<TModel> filterModel, FilterConditions<ApplicationLog> filterConditions,
        Expression<Func<ApplicationLog, TModel>> mapping,
        OrderCondition<ApplicationLog>? orderBy = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Finds an entity with the given primary key values. if no entity found returns null
    /// </summary>
    /// <param name="id">primary key of the entity</param>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>founded entity or null if no entity found</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<ApplicationLog?> GetByIdAsync(ObjectId id,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Begins tracking the given entity in the Deleted state such that it will be removed from the database when <see cref="SaveChangesAsync"/>> is called
    /// </summary>
    /// <param name="entity">the entity to remove</param>
    void Delete(ApplicationLog entity);

    /// <summary>
    /// Begins tracking the given entities in the Deleted state such that it will be removed from the database when <see cref="SaveChangesAsync"/>> is called
    /// </summary>
    /// <param name="entities">a list of entities to remove</param>
    void DeleteRange(List<ApplicationLog> entities);

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <exception cref="DbUpdateConcurrencyException">An error is encountered while saving to the database.</exception>
    /// <exception cref="OperationCanceledException">
    /// A concurrency violation is encountered while saving to the database. A concurrency violation occurs when an unexpected number of rows are affected during save. This is usually because the data in the database has been modified since it was loaded into memory.
    /// </exception>
    /// <exception cref="CancellationToken">If the <see cref="DbUpdateException" /> is canceled.</exception>
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    ///  gets the list of all entities inside a table from the database
    /// </summary>
    /// <param name="cancellationToken">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>a list of all entities within the table inside database</returns>
    /// <exception cref="OperationCanceledException">If the <see cref="CancellationToken" /> is canceled.</exception>
    Task<List<ApplicationLog>> GetAllAsync(CancellationToken cancellationToken = default);


}