using FocusCycle.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FocusCycle.Services.RLServices
{
    internal static class ServicesRegistrator
    {
        public static IServiceCollection AddServices(this IServiceCollection services) => services
            .AddSingleton<IOpenWindows, OpenWindowsService>()
            .AddSingleton<IMessageBus, MessageBusService>()
            ;
    }
}
