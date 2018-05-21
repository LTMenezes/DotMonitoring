using DotMonitoring.Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace DotMonitoring.Core.Middlewares
{
    public class CollectRequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly MonitoringData _monitoringData;

        public CollectRequestMiddleware(RequestDelegate next, MonitoringData monitoringData)
        {
            this._next = next;
            this._monitoringData = monitoringData;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string requestPath = httpContext.Request.Path;
            DateTimeOffset dt = DateTimeOffset.Now;

            Task nextPipelineAction = this._next.Invoke(httpContext);

            try
            {
                RequestData req = new RequestData(dt, requestPath);
                this._monitoringData.CollectRequestData(req);
            }
            catch (Exception ex)
            {
                // We cannot do anything here to solve the exception, perhaps we could log the error in the future.
            }

            await nextPipelineAction;
            return;
        }
    }
}

