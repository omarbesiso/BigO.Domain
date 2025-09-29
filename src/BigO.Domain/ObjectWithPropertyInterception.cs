using System.Runtime.CompilerServices;

namespace BigO.Domain;

/// <summary>
///     Base class for objects requiring property setting interception. This class provides a mechanism to intercept
///     property
///     changes, allowing for custom logic such as tracking, validation, or other operations when properties are set.
/// </summary>
public abstract class ObjectWithPropertyInterception
{
    /// <summary>
    ///     Sets the field to the specified value. This method can be overridden to introduce custom logic when a property is
    ///     set,
    ///     such as tracking changes, validating the new value, or performing other operations.
    /// </summary>
    /// <typeparam name="TFieldType">The type of the field.</typeparam>
    /// <param name="field">The field to set.</param>
    /// <param name="value">The value to set.</param>
    /// <param name="propertyName">The name of the property. This is automatically provided by the compiler.</param>
    /// <returns>
    ///     <c>true</c> if the field is set to a new value; <c>false</c> if the new value is the same as the current
    ///     value.
    /// </returns>
    /// <remarks>
    ///     This method compares the current value of the field with the new value using the default equality comparer for the
    ///     field type.
    ///     If the values are different, the field is updated and the method returns <c>true</c>. If the values are the same,
    ///     the field
    ///     is not updated and the method returns <c>false</c>.
    ///     Overriding this method allows for additional behavior to be executed when a property is set, such as logging
    ///     property changes,
    ///     triggering events, or enforcing business rules.
    /// </remarks>
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