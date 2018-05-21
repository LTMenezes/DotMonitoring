using DotMonitoring.Core.Models;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DotMonitoring.Core.Monitors
{
    public class ProcessInformationMonitor
    {
        private readonly MonitoringData _monitoringData;

        public ProcessInformationMonitor(MonitoringData monitoringData)
        {
            this._monitoringData = monitoringData;
        }

        public void GatherProcessInformation()
        {
            Process currentProcess = Process.GetCurrentProcess();

            long peakVmMemory = currentProcess.PeakVirtualMemorySize64;
            long vmMemory = currentProcess.PagedMemorySize64;
            int numberOfThreads = currentProcess.Threads.Count;
            double TotalUptimeInSeconds = (DateTime.Now - currentProcess.StartTime).TotalSeconds;
            long workingSet = currentProcess.WorkingSet64;

            ProcessData processData = new ProcessData(peakVmMemory, vmMemory, numberOfThreads, TotalUptimeInSeconds, workingSet);
            this._monitoringData.ProcessData.Add(processData);
        }

        public async void StartMonitoring()
        {
            while (true)
            {
                GatherProcessInformation();
                await Task.Delay(5000);
            }
        }
    }
}
