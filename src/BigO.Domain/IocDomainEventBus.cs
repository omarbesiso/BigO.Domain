using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace BigO.Domain;

/// <summary>
///     A default implementation of the <see cref="IDomainEventBus" /> using IOC to deliver published events to the
///     relevant handlers.
/// </summary>
[PublicAPI]
internal class IocDomainEventBus : IDomainEventBus
{
    private readonly IServiceProvider _serviceProvider;

    public IocDomainEventBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task Publish<TDomainEvent>(TDomainEvent @event) where TDomainEvent : class
    {
        var services = _serviceProvider.GetServices(typeof(IDomainEventHandler<TDomainEvent>))
            .Cast<IDomainEventHandler<TDomainEvent>>().ToList();

        if (!services.Any())
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