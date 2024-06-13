using JetBrains.Annotations;

namespace BigO.Domain;

/// <summary>
/// Interface defining a unit of work implementation, which maintains a list of objects affected by a business transaction and coordinates the writing out of changes and the resolution of concurrency problems.
/// </summary>
[PublicAPI]
public interface IUnitOfWork
{
    /// <summary>
    /// Returns the repository implementation of the specified type.
    /// </summary>
    /// <typeparam name="TRepository">The type of the repository. Must implement <see cref="IRepository"/>.</typeparam>
    /// <returns>An instance of the specified repository type.</returns>
    TRepository Repository<TRepository>() where TRepository : class, IRepository;

    /// <summary>
    /// Commits the changes that occurred within the scope of the unit of work.
    /// </summary>
    /// <remarks>
    /// This method saves all the changes made within the current unit of work context to the database. It is intended for synchronous operations.
    /// </remarks>
    void Commit();

    /// <summary>
    /// Asynchronously commits the changes that occurred within the scope of the unit of work.
    /// </summary>
    /// <remarks>
    /// This method saves all the changes made within the current unit of work context to the database. It is intended for asynchronous operations.
    /// </remarks>
    /// <returns>A task that represents the asynchronous commit operation.</returns>
    Task CommitAsync();
}