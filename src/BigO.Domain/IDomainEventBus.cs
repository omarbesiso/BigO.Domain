﻿using JetBrains.Annotations;

namespace BigO.Domain;

/// <summary>
///     Defines a generic event bus for sending domain events.
/// </summary>
/// <remarks>
///     The default implementation delivers domain events to registered domain event handlers. There could be
///     multiple domain event handlers for the same domain event.
/// </remarks>
[PublicAPI]
public interface IDomainEventBus
{
    /// <summary>
    ///     Publishes the specified domain event. Delivers the event to the registered/subscribed domain event handlers.
    /// </summary>
    /// <typeparam name="TDomainEvent">The type of the domain event.</typeparam>
    /// <param name="event">The domain event.</param>
    Task Publish<TDomainEvent>(TDomainEvent @event)
        where TDomainEvent : IDomainEvent;
}