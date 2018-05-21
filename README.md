# DotMonitoring

A simple, lightweight webserver monitor for .net core applications.

![Alt Text](/docs/dashboard.gif)

DotMonitoring gives you two endpoints '/monitoring' for the dashboard UI and '/monitoring/data.json' for a json file with information about the webserver, making it easier to integrate DotMonitoring with existing monitoring applications and avoiding any kind of vendor lock-in.

This package is still under construction, the first release should be in the upcoming weeks.

## Usage

To start using DotMonitoring you only need to add two lines to your Startup class:

```csharp
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMonitoring();
        services.AddMvc();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseMonitoring();
        app.UseMvc();
    }
```

## Contributing

Pull requests are always welcome and you can reach me by opening an issue on github.
