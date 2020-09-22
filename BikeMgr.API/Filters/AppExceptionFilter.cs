using BikeMgr.API.Helpers;
using BikeMgr.Core;
using BikeMgr.Core.Interface.Utilities;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Unity;

namespace BikeMgrAPI.Filters
{
    public class AppExceptionFilter : ExceptionFilterAttribute
    {
        private ILogService _log;

        public AppExceptionFilter()
        {
            _log = UnityHelpers.GetConfiguredContainer().Resolve<ILogService>();
        }
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            string message = string.Empty;

            switch (actionExecutedContext.Exception)
            {
                case NotFoundException nfex:
                    _log.LogError(nfex, nfex.Message);
                    status = HttpStatusCode.NotFound;
                    message = nfex.Message;
                    break;
                default:
                    _log.LogError(actionExecutedContext.Exception, actionExecutedContext.Exception.Message);
                    status = HttpStatusCode.InternalServerError;
                    message = actionExecutedContext.Exception.Message;
                    break;
            }
            actionExecutedContext.Response = new HttpResponseMessage()
            {
                Content = new StringContent(message, System.Text.Encoding.UTF8, "text/plain"),
                StatusCode = status
            };
            base.OnException(actionExecutedContext);
        }
    }
}