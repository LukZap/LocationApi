using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using LocationApi.Mappings;
using LocationApi.Models;
using LocationApi.Services;
using LocationApi.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace LocationApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterWebApiFilterProvider(config); // todo dodaæ exception filter
            builder.RegisterWebApiModelBinderProvider(); // spr

            // register services
            builder.RegisterType<GeolocationService>().As<IGeolocationService>();
            builder.Register(_ => _createLocationDbSettings()).As<ILocationDbSettings>().SingleInstance();
            builder.Register(c => {
                var mongoDbSettings = c.Resolve<ILocationDbSettings>();
                var client = new MongoClient(mongoDbSettings.ConnectionString);
                return client.GetDatabase(mongoDbSettings.DatabaseName);
            }).As<IMongoDatabase>();

            //register mappings
            builder.Register(_ => new MapperConfiguration(cfg => {
                cfg.AddProfile<GeolocationViewModelProfile>();
            }))
                .As<IConfigurationProvider>()
                .SingleInstance();
            builder.Register(c => c.Resolve<IConfigurationProvider>().CreateMapper())
                .As<IMapper>();

            var container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        private ILocationDbSettings _createLocationDbSettings()
        {
            return new LocationDbSettings
            {
                GeolocationsCollection = ConfigurationManager.AppSettings["GeolocationsCollection"],
                ConnectionString = ConfigurationManager.AppSettings["ConnectionString"],
                DatabaseName = ConfigurationManager.AppSettings["DatabaseName"]
            };
        }
    }
}
