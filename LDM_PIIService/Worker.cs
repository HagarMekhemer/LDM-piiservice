using LDM_PIIService.DSL;
using LDM_PIIService.Helpers;
using Microsoft.Extensions.Options;

namespace LDM_PIIService
{
    public class Worker : BackgroundService
    {
        private readonly TimersDSL _timersDSL;

        public Worker(TimersDSL timersDSL, ConfigManager _config)
        {
            FileLogger.GetLogFilePath_Event += () => _config.LogPath;
            _timersDSL = timersDSL;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timersDSL.Start(stoppingToken);
        }
    }
}
        
