using BikeMgrWeb.Loggers;
using System.Web.Mvc;
using Unity;

namespace BikeMgrWeb.Filters
{
    public class AppExceptionFilter : HandleErrorAttribute
    {
        private readonly ILogService _log;

        public AppExceptionFilter()
        {
            _log = UnityConfig.Container.Resolve<ILogService>();

        }
        public override void OnException(ExceptionContext filterContext)
        {
            _log.LogError(filterContext.Exception, filterContext.Exception.Message);
            base.OnException(filterContext);
        }
    }
}