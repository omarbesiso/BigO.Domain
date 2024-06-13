using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace BigO.Domain;

/// <summary>
///     A default implementation of the <see cref="IDomainEventBus" /> using IOC to deliver published events to the
///     relevant handlers.
/// </summary>
/// <param name="serviceProvider">The service provider to resolve event handlers.</param>
[PublicAPI]
internal class IocDomainEventBus(IServiceProvider serviceProvider) : IDomainEventBus
{
    /// <summary>
    /// Publishes the specified domain event to all registered event handlers.
    /// </summary>
    /// <typeparam name="TDomainEvent">The type of the domain event.</typeparam>
    /// <param name="event">The domain event to publish.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when no handlers are found for the event type.</exception>
    public async Task Publish<TDomainEvent>(TDomainEvent @event) where TDomainEvent : class
    {
        var services = serviceProvider.GetServices(typeof(IDomainEventHandler<TDomainEvent>))
            .Cast<IDomainEventHandler<TDomainEvent>>().ToList();

        if (services.Count == 0)
        {
            var eventType = typeof(TDomainEvent).FullName;
            throw new ArgumentOutOfRangeException(eventType,
                $"No registration was found for any event handlers to handle the event: {eventType}");
        }

        foreach (var domainEventHandler in services)
        {
            await domainEventHandler.Handle(@event);
        }
    }
}