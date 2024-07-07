using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace BigO.Domain;

/// <summary>
///     A default implementation of the <see cref="IDomainEventBus" /> using IOC to deliver published events to the
///     relevant handlers.
/// </summary>
/// <remarks>
///     Initializes a new instance of the <see cref="IocDomainEventBus" /> class.
/// </remarks>
/// <param name="serviceProvider">The service provider to resolve event handlers.</param>
[PublicAPI]
internal class IocDomainEventBus(IServiceProvider serviceProvider) : IDomainEventBus
{
    /// <summary>
    ///     Publishes the specified domain event to all registered event handlers.
    /// </summary>
    /// <typeparam name="TDomainEvent">The type of the domain event.</typeparam>
    /// <param name="domainEvent">The domain event to publish.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when no handlers are found for the event type.</exception>
    public async Task Publish<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent
    {
        var handlers = serviceProvider.GetServices<IDomainEventHandler<TDomainEvent>>().ToList();

        if (handlers.Count == 0)
        {
            var eventType = typeof(TDomainEvent).FullName;
            var errorMessage = $"No registered handlers found for event type: {eventType}";
            throw new ArgumentOutOfRangeException(eventType, errorMessage);
        }

        foreach (var handler in handlers)
        {
            await handler.Handle(domainEvent);
        }
    }
}