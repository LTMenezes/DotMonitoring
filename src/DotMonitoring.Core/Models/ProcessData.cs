using System;

namespace DotMonitoring.Core.Models
{
    public class ProcessData
    {
        public long PeakVirtualMachineMemory { get; private set; }

        public long VirtualMachineMemory { get; private set; }

        public long WorkingSet { get; private set; }

        public int NumberOfThreads { get; set; }

        public double TotalUptimeInSeconds { get; set; }

        public long Timestamp { get; set; }

        public ProcessData(long peakVirtualMachineMemory, long virtualMachineMemory, int numberOfThreads, double totalUptimeInSeconds, long workingSet)
        {
            this.PeakVirtualMachineMemory = peakVirtualMachineMemory;
            this.VirtualMachineMemory = virtualMachineMemory;
            this.NumberOfThreads = numberOfThreads;
            this.TotalUptimeInSeconds = totalUptimeInSeconds;
            this.Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            this.WorkingSet = workingSet;
        }
    }
}
