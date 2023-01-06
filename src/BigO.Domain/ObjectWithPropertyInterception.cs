using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace BigO.Domain;

/// <summary>
///     Base class for objects requiring property setting interception.
/// </summary>
[PublicAPI]
public abstract class ObjectWithPropertyInterception
{
    /// <summary>
    ///     Sets the field. Can be overridden to introduce setting interceptors for purposes of tracking, validation or other.
    /// </summary>
    /// <typeparam name="TFieldType">The type of the field.</typeparam>
    /// <param name="field">The field to set.</param>
    /// <param name="value">The value to set.</param>
    /// <param name="propertyName">Name of the property.</param>
    /// <returns><c>true</c> if the property is set to a new value, <c>false</c> otherwise.</returns>
    protected virtual bool SetField<TFieldType>(ref TFieldType field, TFieldType value,
        [CallerMemberName] string propertyName = "")
    {
        if (EqualityComparer<TFieldType>.Default.Equals(field, value))
        {
            return false;
        }

        field = value;
        return true;
    }
}