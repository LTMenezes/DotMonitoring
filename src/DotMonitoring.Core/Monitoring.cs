using DotMonitoring.Core.Middlewares;
using DotMonitoring.Core.Models;
using DotMonitoring.Core.Monitors;
using DotMonitoring.Core.WebInterfaceFiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DotMonitoring.Core
{
    public static class Monitoring
    {
        public static IServiceCollection AddMonitoring(this IServiceCollection services)
        {
            services.AddSingleton<MonitoringData>();
            services.AddSingleton<ProcessInformationMonitor>();
            services.AddSingleton<WebInterfaceFileManager>();

            return services;
        }

        public static IApplicationBuilder UseMonitoring(this IApplicationBuilder app)
        {
            // Setup monitoring data endpoints
            app.UseMiddleware<ServeMonitoringDataMiddleware>();

            // Setup api requests monitoring
            app.UseMiddleware<CollectRequestMiddleware>();
            
            // Setup process information monitoring
            app.ApplicationServices.GetService<ProcessInformationMonitor>().StartMonitoring();
            return app;
        }
    }
}
