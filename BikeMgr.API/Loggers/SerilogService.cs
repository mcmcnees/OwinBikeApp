using BikeMgr.Core.Interface.Utilities;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BikeMgrAPI.Loggers
{
    public class SerilogService : ILogService
    {
        private readonly Logger _logger;

        public SerilogService()
        {
            _logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .WriteTo.File(HttpContext.Current.Server.MapPath("~/logs/log-.txt"), rollingInterval: RollingInterval.Day)
                        .CreateLogger();
        }

        public void LogError(string format, params object[] parameters)
        {
            _logger.Error(format, parameters);
        }

        public void LogError(Exception ex, string format, params object[] parameters)
        {
            _logger.Error(ex, format, parameters);
        }

        public void LogInformation(string format, params object[] parameters)
        {
            _logger.Information(format, parameters);
        }

        public void LogWarning(string format, params object[] parameters)
        {
            _logger.Warning(format, parameters);
        }
    }
}