using BikeMgr.Core.Interface.Queries;
using BikeMgr.Core.Interface.Services;
using BikeMgr.Core.Interface.Utilities;
using BikeMgr.Core.Services;
using BikeMgr.Infrastructure;
using BikeMgr.Infrastructure.Queries;
using BikeMgr.Infrastructure.Services;
using BikeMgrAPI.Loggers;
using System;
using System.Data.Entity;
using System.Linq;
using Unity;
using Unity.Injection;

namespace BikeMgr.API.Helpers
{
    public static class UnityHelpers
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        public static void RegisterTypes(IUnityContainer container)
        {
            var myAssemblies = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("SelfHostWebApiOwin")).ToArray();

            container.RegisterType(typeof(Startup));

            container.RegisterType<DbContext, DataContext>();
            container.RegisterType<IBikeQueries, BikeQueries>();
            container.RegisterType<IBikeService, BikeService>();
            container.RegisterType<ILogService, SerilogService>();
            container.RegisterType<IStorageService, FileService>(new InjectionConstructor(
                new FileService.Options() { RootPath = System.Web.HttpContext.Current.Server.MapPath("~") }
                ));
        }

    }
}