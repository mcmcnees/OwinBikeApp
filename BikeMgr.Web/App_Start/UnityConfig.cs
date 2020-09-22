using BikeMgrWeb.Loggers;
using BikeMgrWeb.Services;
using System;
using System.Configuration;
using Unity;
using Unity.Injection;

namespace BikeMgrWeb
{
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        public static IUnityContainer Container => container.Value;
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            container.RegisterType<HttpService>(new InjectionConstructor(
                new HttpService.Options() { ApiUrl = ConfigurationManager.AppSettings["BikeApi.Uri"] }
                ));
            container.RegisterType<ILogService, SerilogService>();
        }
    }
}