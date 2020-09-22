using BikeMgr.API.Helpers;
using BikeMgr.Core.Interface.Utilities;
using System.Net.Http;
using System.Text;
using System.Web.Http.ExceptionHandling;
using Unity;

namespace BikeMgr.API.Loggers
{
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        private readonly ILogService _log;

        public UnhandledExceptionLogger()
        {
            _log = UnityHelpers.GetConfiguredContainer().Resolve<ILogService>();
        }
        public override void Log(ExceptionLoggerContext context)
        {
            _log.LogError(context.Exception, context.Exception.Message);
        }

        private static string RequestToString(HttpRequestMessage request)
        {
            var message = new StringBuilder();
            if (request.Method != null)
                message.Append(request.Method);

            if (request.RequestUri != null)
                message.Append(" ").Append(request.RequestUri);

            return message.ToString();
        }
    }
}
