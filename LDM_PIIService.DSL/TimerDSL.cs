using LDM_PIIService.DSL;
using LDM_PIIService.Entities.RequestsDTOs;
using LDM_PIIService.Entities.ResponsesDTOs;
using LDM_PIIService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Timer = System.Timers.Timer;

namespace LDM_PIIService.DSL
{
    public class TimersDSL
    {
        private readonly List<Timer> timersList = new List<Timer>();
        private readonly FileLogger _logger;
        private readonly ConfigManager _configManager;
        private readonly Get_GH_Attachment_API_DSL _getDsl;
        private readonly Set_GH_Attachment_API_DSL _setDsl;

        public TimersDSL(
            ConfigManager configManager,
            Get_GH_Attachment_API_DSL getDsl,
            Set_GH_Attachment_API_DSL setDsl)
        {
            _logger = FileLogger.GetInstance("TimersDSL");
            _configManager = configManager;
            _getDsl = getDsl;
            _setDsl = setDsl;
        }

        public void Start(CancellationToken stoppingToken)
        {
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Worker started at: {DateTimeOffset.Now}");

            if (timersList.Any())
            {
                StopTimers();
            }

            var mainTimer = InitTimer(async () => await ProcessAttachmentAsync(), 5, 30);
            timersList.Add(mainTimer);
            mainTimer.Start();
        }

        private Timer InitTimer(Func<Task> timerProcessAsync, int startAfterSeconds, double intervalInSeconds)
        {
            var timer = new Timer
            {
                Interval = startAfterSeconds * 1000,
                AutoReset = false,
                Enabled = true
            };

            timer.Elapsed += async (sender, e) =>
            {
                try
                {
                    timer.Enabled = false;
                    await timerProcessAsync();
                }
                catch (Exception ex)
                {
                    _logger.WriteToLogFile(ActionTypeEnum.Exception, $"Timer error: {ex.Message}");
                }
                finally
                {
                    timer.Interval = intervalInSeconds * 1000;
                    timer.Enabled = true;
                }
            };

            return timer;
        }

        private async Task ProcessAttachmentAsync()
        {
            _logger.WriteToLogFile(ActionTypeEnum.Information, "Processing attachment timer task...");

            var request = _getDsl.GetAttachment();
            if (request == null || string.IsNullOrWhiteSpace(request.Pdf))
            {
                _logger.WriteToLogFile(ActionTypeEnum.Warning, "No valid attachment request returned.");
                return;
            }

            var response = await _setDsl.UpdatePdfStatusAsync(request);
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"PDF Update Result: {response?.Status}");
        }

        public void StopTimers()
        {
            try
            {
                foreach (var timer in timersList)
                {
                    timer?.Stop();
                    timer?.Dispose();
                }
                timersList.Clear();
            }
            catch (Exception ex)
            {
                _logger.WriteToLogFile(ActionTypeEnum.Exception, $"Error stopping timers: {ex.Message}");
            }
        }
    }
}

