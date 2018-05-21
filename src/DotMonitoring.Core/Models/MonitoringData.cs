using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotMonitoring.Core.Models
{
    public class MonitoringData
    {
        public IList<RequestData> RequestsData { get; set; }
        public IList<ProcessData> ProcessData { get; set; }
        public long Timestamp => DateTimeOffset.Now.ToUnixTimeMilliseconds();

        private int TotalSecondsToStore { get; set; }
        private Task BackgroundGarbageCollectorTask { get; }
        private readonly SemaphoreSlim RequestDataCollectionLock = new SemaphoreSlim(1, 1);

        public MonitoringData()
        {
            this.TotalSecondsToStore = 120;
            this.RequestsData = new List<RequestData>();
            this.ProcessData = new List<ProcessData>();
            this.BackgroundGarbageCollectorTask = new Task(CleanOldRequestData);
            this.BackgroundGarbageCollectorTask.Start();
        }

        public async void CollectRequestData(RequestData request)
        {
            // We need to syncronize saving the request because we have multiple instances collecting data
            // simultaneously and if we cannot guarantee order it will be harder to evict data later.
            await this.RequestDataCollectionLock.WaitAsync();

            try
            {
                this.RequestsData.Add(request);
            }
            finally
            {
                this.RequestDataCollectionLock.Release();
            }
        }

        public async void CleanOldRequestData()
        {
            while (true)
            {
                long currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                for (int i = 0; i < this.RequestsData.Count; i++)
                {
                    long timeDifference = currentTimestamp - this.RequestsData[i].Timestamp;
                    if (timeDifference >= this.TotalSecondsToStore)
                    {
                        this.RequestsData.RemoveAt(i);
                    }
                    else
                    {
                        //Because we are reading the list from the begging and older data should be at the begging.
                        break;
                    }
                }

                for (int i = 0; i < this.ProcessData.Count; i++)
                {
                    long timeDifference = currentTimestamp - this.ProcessData[i].Timestamp;
                    if (timeDifference >= this.TotalSecondsToStore)
                    {
                        this.ProcessData.RemoveAt(i);
                    }
                    else
                    {
                        //Because we are reading the list from the begging and older data should be at the begging.
                        break;
                    }
                }

                await Task.Delay(10000);
            }
        }
    }
}
