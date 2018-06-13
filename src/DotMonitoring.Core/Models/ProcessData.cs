using System;

namespace DotMonitoring.Core.Models
{
    public class ProcessData
    {
        public string Architecture { get; set; }

        public string OsDescription { get; set; }

        public string MachineName { get; set; }

        public long PagedMemorySize { get; set; }

        public long VirtualMemorySize { get; set; }

        public long WorkingSet { get; set; }

        public int NumberOfThreads { get; set; }

        public double TotalUptimeInSeconds { get; set; }

        public long Timestamp { get; private set; }

        public ProcessData()
        {
            this.Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }
}
