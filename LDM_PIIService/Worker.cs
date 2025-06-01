using LDM_PIIService.Helpers;
using Microsoft.Extensions.Options;

namespace LDM_PIIService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly int _intervalInMinutes;
        private readonly FileLogger _fileLogger;

        public Worker(ILogger<Worker> logger , ConfigManager configManager)
        {
            _logger = logger;
            _intervalInMinutes = configManager.IntervalInMinutes;
            FileLogger.GetLogFilePath_Event += () => configManager.LogPath;
            _fileLogger = FileLogger.GetInstance("Worker");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    _fileLogger.WriteToLogFile(ActionTypeEnum.Information, $"Worker executed at {DateTime.Now}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Processing failed");
                    _fileLogger.WriteToLogFile(ex, "Unhandled exception in Worker loop");
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromMinutes(_intervalInMinutes), stoppingToken);
                }
            }
        }
    }
}
