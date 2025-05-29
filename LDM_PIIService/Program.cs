using LDM_PIIService;
using LDM_PIIService.Helpers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<TimeLogger>(hostContext.Configuration.GetSection("TimeLogger"));
        services.AddSingleton<ConfigManager>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
