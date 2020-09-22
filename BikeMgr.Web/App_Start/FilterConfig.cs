using BikeMgrWeb.Filters;
using System.Web.Mvc;
using Thinktecture.IdentityModel.Mvc;

namespace BikeMgrWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // Display ~/Shared/Error
            //filters.Add(new HandleErrorAttribute());
            filters.Add(new AppExceptionFilter());
            // Display ~/Shared/Forbidden
            filters.Add(new HandleForbiddenAttribute());
        }
    }
}
