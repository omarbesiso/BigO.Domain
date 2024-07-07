﻿using JetBrains.Annotations;

namespace BigO.Domain;

/// <summary>
///     Defines the contract for a domain event handler as defined in Domain Driven Design.
/// </summary>
/// <remarks>
///     1. When an operation causes an aggregate a change to the domain, there might be other areas in the domain that need
///     to
///     respond to that change event taking place.
///     2. Also in Event sourcing domain events can also be used to persist the behavior of aggregates instead of their
///     state. Multiple handlers can exist for the same event.
/// </remarks>
/// <typeparam name="TDomainEvent">The type of the domain event.</typeparam>
[PublicAPI]
public interface IDomainEventHandler<in TDomainEvent> where TDomainEvent : IDomainEvent
{
    /// <summary>
    ///     Handles the specified domain event published through the <see cref="IDomainEventBus" /> or another mechanism.
    /// </summary>
    Task Handle(TDomainEvent @event);
}