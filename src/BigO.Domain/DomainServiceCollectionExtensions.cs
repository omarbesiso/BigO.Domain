// ReSharper disable UnusedMember.Global

using BigO.Core.Validation;
using BigO.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace BigO.Domain;

/// <summary>
///     Class providing extensions to the <see cref="IServiceCollection" /> to allow for the registration of different
///     types of handlers for different message types.
/// </summary>
public static class DomainServiceCollectionExtensions
{
    /// <summary>
    ///     Registers the specified <typeparamref name="TDomainEventHandler" /> implementation as a service for the
    ///     <typeparamref name="TDomainEvent" /> with the specified <paramref name="serviceLifetime" />.
    /// </summary>
    /// <typeparam name="TDomainEvent">
    ///     The type of domain event for which the <typeparamref name="TDomainEventHandler" /> will
    ///     be registered.
    /// </typeparam>
    /// <typeparam name="TDomainEventHandler">The type of domain event handler that will be registered as a service.</typeparam>
    /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to which the service will be added.</param>
    /// <param name="serviceLifetime">
    ///     The <see cref="ServiceLifetime" /> of the registered service. Default value is
    ///     <see cref="ServiceLifetime.Transient" />.
    /// </param>
    /// <returns>The <paramref name="serviceCollection" /> to which the service was added.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="serviceCollection" /> is <c>null</c>.</exception>
    public static IServiceCollection RegisterDomainEventHandler<TDomainEvent, TDomainEventHandler>(
        this IServiceCollection serviceCollection, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
        where TDomainEvent : IDomainEvent
        where TDomainEventHandler : class, IDomainEventHandler<TDomainEvent>
    {
        Guard.NotNull(serviceCollection);

        return serviceLifetime switch
        {
            ServiceLifetime.Singleton => serviceCollection
                .AddSingleton<IDomainEventHandler<TDomainEvent>, TDomainEventHandler>(),
            ServiceLifetime.Scoped => serviceCollection
                .AddScoped<IDomainEventHandler<TDomainEvent>, TDomainEventHandler>(),
            _ => serviceCollection.AddTransient<IDomainEventHandler<TDomainEvent>, TDomainEventHandler>()
        };
    }

    /// <summary>
    ///     Registers the <see cref="IocDomainEventBus" /> as the default implementation of <see cref="IDomainEventBus" />
    ///     with the specified <paramref name="serviceLifetime" />.
    /// </summary>
    /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to which the service will be added.</param>
    /// <param name="serviceLifetime">
    ///     The <see cref="ServiceLifetime" /> of the registered service. Default value is
    ///     <see cref="ServiceLifetime.Singleton" />.
    /// </param>
    /// <returns>The <paramref name="serviceCollection" /> to which the service was added.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="serviceCollection" /> is <c>null</c>.</exception>
    public static IServiceCollection RegisterDefaultDomainEventBus(this IServiceCollection serviceCollection,
        ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
    {
        Guard.NotNull(serviceCollection);

        return serviceLifetime switch
        {
            ServiceLifetime.Singleton => serviceCollection.AddSingleton<IDomainEventBus, IocDomainEventBus>(),
            ServiceLifetime.Scoped => serviceCollection.AddScoped<IDomainEventBus, IocDomainEventBus>(),
            _ => serviceCollection.AddTransient<IDomainEventBus, IocDomainEventBus>()
        };
    }

    /// <summary>
    ///     Registers all <see cref="IDomainEventHandler{T}" /> implementations in the assembly of
    ///     <typeparamref name="TModule" />
    ///     as services with the specified <paramref name="serviceLifetime" />.
    /// </summary>
    /// <typeparam name="TModule">
    ///     The module for which the <see cref="IDomainEventHandler{T}" /> implementations will be
    ///     registered.
    /// </typeparam>
    /// <param name="serviceCollection">The <see cref="IServiceCollection" /> to which the service will be added.</param>
    /// <param name="serviceLifetime">
    ///     The <see cref="ServiceLifetime" /> of the registered service. Default value is
    ///     <see cref="ServiceLifetime.Scoped" />.
    /// </param>
    /// <returns>The <paramref name="serviceCollection" /> to which the service was added.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="serviceCollection" /> is <c>null</c>.</exception>
    public static IServiceCollection RegisterModuleDomainEventHandlers<TModule>(
        this IServiceCollection serviceCollection,
        ServiceLifetime serviceLifetime = ServiceLifetime.Scoped) where TModule : IModule
    {
        Guard.NotNull(serviceCollection);

        var type = typeof(IDomainEventHandler<>);
        RegisterInternal<TModule>(serviceCollection, serviceLifetime, type);
        return serviceCollection;
    }

    private static void RegisterInternal<TModule>(IServiceCollection serviceCollection, ServiceLifetime serviceLifetime,
        Type type) where TModule : IModule
    {
        Guard.NotNull(serviceCollection);

        switch (serviceLifetime)
        {
            case ServiceLifetime.Scoped:
                serviceCollection.Scan(scan =>
                    scan.FromAssemblyOf<TModule>()
                        .AddClasses(classes => classes.AssignableTo(type))
                        .AsImplementedInterfaces()
                        .WithScopedLifetime());
                break;
            case ServiceLifetime.Transient:
                serviceCollection.Scan(scan =>
                    scan.FromAssemblyOf<TModule>()
                        .AddClasses(classes => classes.AssignableTo(type))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime());
                break;
            case ServiceLifetime.Singleton:
                serviceCollection.Scan(scan =>
                    scan.FromAssemblyOf<TModule>()
                        .AddClasses(classes => classes.AssignableTo(type))
                        .AsImplementedInterfaces()
                        .WithSingletonLifetime());
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(serviceLifetime), serviceLifetime,
                    "Invalid service lifetime");
        }
    }
}