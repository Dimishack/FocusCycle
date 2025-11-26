using FocusCycle.Services.RLServices;
using FocusCycle.ViewModels.RLViewModels;
using FocusCycle.Views.Windows;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace FocusCycle
{
    public partial class App : Application
    {
        private static IHost? __host;
        
        public static IHost Host => __host ??= Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder(Environment.GetCommandLineArgs())
            .ConfigureServices((host, services) => services
            .AddViewModels()
            .AddWindows()
            .AddServices())
            .Build();

        public static IServiceProvider Services => Host.Services;

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Host;
            base.OnStartup(e);
            await host.StartAsync();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            using (Host) await Host.StopAsync();
        }
    }
}
