using LDM_PIIService.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
//using System.Timers;
using Timer = System.Timers.Timer;

namespace LDM_PIIService.DSL
{
    public class TimersDSL
    {
        private readonly List<Timer> timersList = new List<Timer>();
        private readonly FileLogger _logger;
        private readonly ConfigManager _configManager;

        public TimersDSL(ConfigManager configManager)
        {
            _logger = FileLogger.GetInstance("TimersDSL");
            _configManager = configManager;
        }

        public void Start(CancellationToken stoppingToken)
        {
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Worker started at: {DateTimeOffset.Now}");

            if (timersList.Any())
            {
                StopTimers();
            }
           
        }

        private Timer InitTimer(Func<Task> timerProcessAsync, int startAfterSeconds, double intervalInSeconds)
        {
            var timer = new Timer()
            {
                Enabled = true,
                Interval = startAfterSeconds * 1000
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
                    _logger.WriteToLogFile(ActionTypeEnum.Exception, $"Exception in timer process: {ex}");
                }
                finally
                {
                    timer.Interval = intervalInSeconds * 1000;
                    timer.Enabled = true;
                }
            };

            return timer;
        }

        public void StopTimers()
        {
            try
            {
                foreach (var timer in timersList)
                {
                    Stop(timer);
                }
                timersList.Clear();
            }
            catch (Exception ex)
            {
                _logger.WriteToLogFile(ActionTypeEnum.Exception, $"Exception stopping timers: {ex}");
            }
        }

        private void Stop(Timer timer)
        {
            if (timer != null)
            {
                timer.Dispose();
            }
        }
    }
}

