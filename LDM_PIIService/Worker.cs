using LDM_PIIService.Helpers;
using Microsoft.Extensions.Options;

namespace LDM_PIIService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly int _intervalMilliseconds;
        private readonly FileLogger _fileLogger;

        public Worker(ILogger<Worker> logger , IOptions<TimeLogger> settings)
        {
            _logger = logger;
            _intervalMilliseconds = settings.Value.IntervalMilliseconds;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    //
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Processing failed");
                }
                finally
                {
                    await Task.Delay(_intervalMilliseconds, stoppingToken);
                }
            }
        }
    }
}
