using DotMonitoring.Core.Models;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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

            ProcessData processData = new ProcessData()
            {
                Architecture = RuntimeInformation.ProcessArchitecture.ToString(),
                OsDescription = RuntimeInformation.OSDescription,
                MachineName = currentProcess.MachineName,
                NumberOfThreads = currentProcess.Threads.Count,
                TotalUptimeInSeconds = (DateTime.Now - currentProcess.StartTime).TotalSeconds,
                WorkingSet = currentProcess.WorkingSet64,
                PagedMemorySize = currentProcess.PagedMemorySize64,
                VirtualMemorySize = currentProcess.VirtualMemorySize64
            };

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
