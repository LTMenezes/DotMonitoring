window.onload = function startChart() {
    var Chart = require(['/monitoring/assets/js/vendors/chart.bundle.min.js'], function(Chart){
    var rchart = document.getElementById("RequestsChart").getContext('2d');
    window.requestsChart = new Chart(rchart, {
        type: 'line',
        data: {
            datasets: [{
                label: 'Number of API calls',
                data: [],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255,99,132,1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 0
            }]
        },
        options: {
            scales: {
                xAxes: [{
                    type: 'time',
                    distribution: 'linear',
                    time: {
                        displayFormats: {
                            millisecond: 'h:mm:ss.SSS a'
                        }
                    }
                }],
                yAxes: [{
                    ticks: {
                        beginAtZero:true
                    }
                }]
            }
        }
    });

    var vmmchart = document.getElementById("VmMemoryUsageChart").getContext('2d');
    window.vmmChart = new Chart(vmmchart, {
        type: 'line',
        data: {
            datasets: [{
                label: 'Virtual Machine Used Memory',
                data: [],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255,99,132,1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 0
            }]
        },
        options: {
            scales: {
                xAxes: [{
                    type: 'time',
                    distribution: 'linear',
                    time: {
                        displayFormats: {
                            millisecond: 'h:mm:ss.SSS a'
                        }
                    }
                }],
                yAxes: [{
                    ticks: {
                        beginAtZero:true
                    }
                }]
            }
        }
    });

    var tchart = document.getElementById("ThreadsChart").getContext('2d');
    window.tChart = new Chart(tchart, {
        type: 'line',
        data: {
            datasets: [{
                label: 'Number of Threads',
                data: [],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255,99,132,1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 0
            }]
        },
        options: {
            scales: {
                xAxes: [{
                    type: 'time',
                    distribution: 'linear',
                    time: {
                        displayFormats: {
                            millisecond: 'h:mm:ss.SSS a'
                        }
                    }
                }],
                yAxes: [{
                    ticks: {
                        beginAtZero:true
                    }
                }]
            }
        }
    });

    var wschart = document.getElementById("WorkingSetChart").getContext('2d');
    window.wschart = new Chart(wschart, {
        type: 'line',
        data: {
            datasets: [{
                label: 'Allocated Physical Memory',
                data: [],
                backgroundColor: [
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 206, 86, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(153, 102, 255, 0.2)',
                    'rgba(255, 159, 64, 0.2)'
                ],
                borderColor: [
                    'rgba(255,99,132,1)',
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 206, 86, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(153, 102, 255, 1)',
                    'rgba(255, 159, 64, 1)'
                ],
                borderWidth: 0
            }]
        },
        options: {
            scales: {
                xAxes: [{
                    type: 'time',
                    distribution: 'linear',
                    time: {
                        displayFormats: {
                            millisecond: 'h:mm:ss.SSS a'
                        }
                    }
                }],
                yAxes: [{
                    ticks: {
                        beginAtZero:true
                    }
                }]
            }
        }
    });

    window.lastReadTimestamp = 0;
    UpdateChartData();
    window.setInterval(UpdateChartData, 1000);
    window.setInterval(UpdateUptime, 1000);
});
};

function UpdateChartData(){
    require(['jquery'], function($){
        $.get( "/monitoring/data.json", function( data ) {
            UpdateRequestData(data.RequestsData);
            UpdateVMMemoryData(data.ProcessData);
            UpdateNThreadsData(data.ProcessData);
            UpdateWorkingSetData(data.ProcessData);
            UpdateUptimeWithRequest(data.ProcessData);

            EvictOldData();

            window.lastReadTimestamp = data.Timestamp;
        });
    });
}

function UpdateUptimeWithRequest(RequestsData){
    if(document.getElementById("UptimeDisplay").innerHTML != ""){
        return;
    }

    var maxUptime = 0;
    RequestsData.forEach(element => {
        if(element.Timestamp < window.lastReadTimestamp){
            return;
        }

        if(element.TotalUptimeInSeconds > maxUptime){
            maxUptime = element.TotalUptimeInSeconds;
        }
    });
    
    document.getElementById("UptimeDisplay").innerHTML = Math.round(maxUptime);
}

function UpdateRequestData(RequestsData){
    var bucketOfCallsPerSecond = {};

    RequestsData.forEach(element => {
        if(element.Timestamp < window.lastReadTimestamp){
            return;
        }

        var elementTimestampInSeconds = Math.round((new Date(element.Timestamp)).getTime() / 1000);

        if(bucketOfCallsPerSecond[elementTimestampInSeconds] == undefined){
            bucketOfCallsPerSecond[elementTimestampInSeconds] = 1;
        }
        else{
            bucketOfCallsPerSecond[elementTimestampInSeconds] = bucketOfCallsPerSecond[elementTimestampInSeconds] + 1;
        }
    });
    console.log(bucketOfCallsPerSecond);
    for(var key in bucketOfCallsPerSecond){
        window.requestsChart.data.datasets[0].data.push({x: new Date(key * 1000), y: bucketOfCallsPerSecond[key], Timestamp: key * 1000})
    }

    window.requestsChart.update();
}

function UpdateVMMemoryData(ProcessData){
    ProcessData.forEach(element => {
        if(element.Timestamp < window.lastReadTimestamp){
            return;
        }

        window.vmmChart.data.datasets[0].data.push({x: new Date(element.Timestamp), y: element.VirtualMachineMemory, Timestamp: element.Timestamp})
    });

    window.vmmChart.update();
}

function UpdateNThreadsData(ProcessData){
    ProcessData.forEach(element => {
        if(element.Timestamp < window.lastReadTimestamp){
            return;
        }

        window.tChart.data.datasets[0].data.push({x: new Date(element.Timestamp), y: element.NumberOfThreads, Timestamp: element.Timestamp})
    });

    window.tChart.update();
}

function UpdateWorkingSetData(ProcessData){
    ProcessData.forEach(element => {
        if(element.Timestamp < window.lastReadTimestamp){
            return;
        }

        window.wschart.data.datasets[0].data.push({x: new Date(element.Timestamp), y: element.WorkingSet, Timestamp: element.Timestamp})
    });

    window.wschart.update();
}

function UpdateUptime(){
    if(document.getElementById("UptimeDisplay").innerHTML == ""){
        return;
    }
  
    document.getElementById("UptimeDisplay").innerHTML = parseInt(document.getElementById("UptimeDisplay").innerHTML) + 1;
}

function EvictOldData(){
    EvictOldDataFromChart(window.requestsChart);
    EvictOldDataFromChart(window.vmmChart);
    EvictOldDataFromChart(window.tChart);
    EvictOldDataFromChart(window.wschart);
}

function EvictOldDataFromChart(chart){
    chart.data.datasets[0].data.forEach(function(element, index, object) {
        if(element.Timestamp < (new Date).getTime() - 300000){
            chart.data.datasets[0].data.splice(index, 1);
        }
    });

    chart.update();
}