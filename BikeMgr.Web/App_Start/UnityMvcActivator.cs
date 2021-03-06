using System.Linq;
using System.Web.Mvc;
using Unity.AspNet.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(BikeMgrWeb.UnityMvcActivator), nameof(BikeMgrWeb.UnityMvcActivator.Start))]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(BikeMgrWeb.UnityMvcActivator), nameof(BikeMgrWeb.UnityMvcActivator.Shutdown))]
namespace BikeMgrWeb
{
    public static class UnityMvcActivator
    {
        public static void Start() 
        {
            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(UnityConfig.Container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(UnityConfig.Container));
        }

        public static void Shutdown()
        {
            UnityConfig.Container.Dispose();
        }
    }
}