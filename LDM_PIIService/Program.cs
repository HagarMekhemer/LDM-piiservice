using LDM_PIIService;
using LDM_PIIService.DSL;
using LDM_PIIService.Helpers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    { 
        services.AddSingleton<ConfigManager>();
        services.AddSingleton<TimersDSL>();
        services.AddHostedService<Worker>();
    })
    .Build();
var configManager = host.Services.GetRequiredService<ConfigManager>();
FileLogger.GetLogFilePath_Event += () => configManager.LogPath;

await host.RunAsync();
