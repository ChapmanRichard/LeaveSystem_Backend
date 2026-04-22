using Microsoft.Extensions.Logging;
using System;

namespace Api.Common.Loggers
{
    public class SeriLogger : IAppLogger
    {
        private ILogger<SeriLogger> logger;
        public SeriLogger(ILogger<SeriLogger> _logger)
        {
            this.logger = _logger;
        }

        public void Info(string message)
        {
            this.logger.LogInformation(message);
        }

        public void Error(Exception ex, string message)
        {
            this.logger.LogError(ex, message);
        }
        public void Debug(string message)
        {
            this.logger.LogDebug(message);
        }
    }
}
