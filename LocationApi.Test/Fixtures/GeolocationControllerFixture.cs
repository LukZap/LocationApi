using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using FluentValidation.WebApi;
using LocationApi.Controllers;
using LocationApi.Exceptions;
using LocationApi.Mappings;
using LocationApi.Models;
using LocationApi.Services;
using LocationApi.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace LocationApi.Test.Fixtures
{
    public class GeolocationControllerFixture : IDisposable
    {
        private readonly MongoDbFixture _dbFixture;
        private readonly ILifetimeScope _scope;

        public GeolocationController Controller { get; }
        public IMongoCollection<Geolocation> GeolocationsCollection { get; }


        public GeolocationControllerFixture()
        {
            _dbFixture = new MongoDbFixture();
            GeolocationsCollection = _dbFixture.Collection;

            var builder = new ContainerBuilder();

            var config = GlobalConfiguration.Configuration;

            FluentValidationModelValidatorProvider.Configure(config);

            builder.RegisterType<GeolocationController>().AsSelf().SingleInstance();
            // register services
            builder.RegisterType<GeolocationService>().As<IGeolocationService>();
            builder.Register(_ => _createLocationDbSettings()).As<ILocationDbSettings>().SingleInstance();
            builder.Register(_ => _dbFixture.MongoClient).As<IMongoClient>().SingleInstance();

            //register mappings
            builder.Register(_ => new MapperConfiguration(cfg => {
                cfg.AddProfile<GeolocationViewModelMappingProfile>();
            }))
                .As<IConfigurationProvider>()
                .SingleInstance();
            builder.Register(c => c.Resolve<IConfigurationProvider>().CreateMapper())
                .As<IMapper>();
            var container = builder.Build();

            _scope = container.BeginLifetimeScope();
            try
            {
                Controller = _scope.Resolve<GeolocationController>();
            }
            catch (Exception)
            {
                Dispose();
                throw;
            }
        }

        public void Dispose()
        {
            _dbFixture.Dispose();
            _scope.Dispose();
        }

        private ILocationDbSettings _createLocationDbSettings()
        {
            return new LocationDbSettings
            {
                GeolocationsCollection = ConfigurationManager.AppSettings["GeolocationsCollection"],
                DatabaseName = ConfigurationManager.AppSettings["DatabaseName"]
            };
        }
    }
}
