namespace BigO.Domain;

/// <summary>
///     A base entity implementation that provides an equality mechanism by comparing unique identifiers and
///     includes infrastructure for setting properties with interception.
/// </summary>
/// <typeparam name="TId">The type of the unique identifier.</typeparam>
/// <remarks>
///     Initializes a new instance of the <see cref="Entity{TId}" /> class.
/// </remarks>
/// <param name="id">The unique identifier for the entity.</param>
public abstract class Entity<TId>(TId id) : ObjectWithPropertyInterception, IEntity<TId>, IEquatable<Entity<TId>>
    where TId : struct
{
    /// <summary>
    ///     Gets the unique identifier of the entity.
    /// </summary>
    public TId Id { get; } = id;

    /// <summary>
    ///     Indicates whether the current entity is equal to another entity of the same type.
    /// </summary>
    /// <param name="other">An entity to compare with this entity.</param>
    /// <returns>
    ///     <see langword="true" /> if the current entity is equal to the <paramref name="other" /> parameter; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public bool Equals(IEntity<TId>? other)
    {
        if (other is null)
        {
            return false;
        }

        return ReferenceEquals(this, other) || Id.Equals(other.Id);
    }

    /// <summary>
    ///     Indicates whether the current entity is equal to another entity of the same type.
    /// </summary>
    /// <param name="other">An entity to compare with this entity.</param>
    /// <returns>
    ///     <see langword="true" /> if the current entity is equal to the <paramref name="other" /> parameter; otherwise,
    ///     <see langword="false" />.
    /// </returns>
    public bool Equals(Entity<TId>? other) => Equals(other as IEntity<TId>);

    /// <summary>
    ///     Determines whether the specified <see cref="object" /> is equal to this entity.
    /// </summary>
    /// <param name="obj">The object to compare with the current entity.</param>
    /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this entity; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == GetType() && Equals((Entity<TId>)obj);
    }

    /// <summary>
    ///     Returns a hash code for this entity.
    /// </summary>
    /// <returns>A hash code for this entity, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode() => Id.GetHashCode();
}