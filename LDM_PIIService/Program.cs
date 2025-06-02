using LDM_PIIService;
using LDM_PIIService.DAL;
using LDM_PIIService.DSL;
using LDM_PIIService.Helpers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    { 
        services.AddSingleton<ConfigManager>();
        services.AddSingleton<TimersDSL>();
        services.AddSingleton<Get_GH_Attachment_API_DAL>();
        services.AddSingleton<Set_GH_Attachment_API_DAL>();
        services.AddSingleton<Get_GH_Attachment_API_DSL>();
        services.AddSingleton<Set_GH_Attachment_API_DSL>();
        services.AddHostedService<Worker>();

    })
    .Build();
await host.RunAsync();
