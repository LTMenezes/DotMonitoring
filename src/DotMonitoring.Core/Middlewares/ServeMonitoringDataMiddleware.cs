using DotMonitoring.Core.Models;
using DotMonitoring.Core.WebInterfaceFiles;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace DotMonitoring.Core.Middlewares
{
    public class ServeMonitoringDataMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MonitoringData _monitoringData;
        private readonly WebInterfaceFileManager _webInterfaceFileManager;

        public ServeMonitoringDataMiddleware(RequestDelegate next, MonitoringData monitoringData, WebInterfaceFileManager webInterfaceFileManager)
        {
            this._next = next;
            this._monitoringData = monitoringData;
            this._webInterfaceFileManager = webInterfaceFileManager;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Path.StartsWithSegments("/monitoring"))
            {
                await this._next.Invoke(httpContext);
                return;
            }

            if (httpContext.Request.Path == "/monitoring/data.json")
            {
                this.SetMonitoringDataResponse(httpContext);
                return;
            }

            if (httpContext.Request.Path == "/monitoring")
            {
                WebInterfaceContent content = await this._webInterfaceFileManager.GetResource("/monitoring/index.html");

                httpContext.Response.StatusCode = 200;
                httpContext.Response.ContentType = content.ContentType;
                await httpContext.Response.WriteAsync(await content.ContentText);

                return;
            }

            if (this._webInterfaceFileManager.DoesResourceExists(httpContext.Request.Path))
            {
                WebInterfaceContent content = await this._webInterfaceFileManager.GetResource(httpContext.Request.Path);

                httpContext.Response.StatusCode = 200;
                httpContext.Response.ContentType = content.ContentType;
                await httpContext.Response.WriteAsync(await content.ContentText);

                return;
            }

            // If the content hasn't been returned by now we cannot resolve it.
            await this._next.Invoke(httpContext);
            return;
        }

        public void SetMonitoringDataResponse(HttpContext httpContext)
        {
            httpContext.Response.StatusCode = 200;
            httpContext.Response.ContentType = "application/json";
            // TODO: We need to improve this logic, this throws an exception because we`re modifying the lists while iterating it.
            httpContext.Response.WriteAsync(JsonConvert.SerializeObject(this._monitoringData));
        }
    }
}
