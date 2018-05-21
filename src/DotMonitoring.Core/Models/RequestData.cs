using System;

namespace DotMonitoring.Core
{
    public class RequestData
    {
        public long Timestamp { get; private set; }

        public string Path { get; private set; }

        public RequestData(DateTimeOffset dateTimeOffset, string requestPath)
        {
            this.Timestamp = dateTimeOffset.ToUnixTimeMilliseconds();
            this.Path = requestPath;
        }
    }
}
