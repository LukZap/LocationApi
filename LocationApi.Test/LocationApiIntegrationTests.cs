using LocationApi.Controllers;
using LocationApi.Exceptions;
using LocationApi.Models;
using LocationApi.Test.Fixtures;
using LocationApi.ViewModels;
using MongoDB.Driver;
using System;
using System.Net.Http;
using System.Web.Http.Results;
using Xunit;

namespace LocationApi.Test
{

    public class LocationApiIntegrationTests : IClassFixture<GeolocationControllerFixture>
    {
        private readonly GeolocationController controller;
        private readonly IMongoCollection<Geolocation> geolocationsCollection;

        public LocationApiIntegrationTests(GeolocationControllerFixture fixture)
        {
            controller = fixture.Controller;
            geolocationsCollection = fixture.GeolocationsCollection;

            // clean collection before each test
            geolocationsCollection.DeleteMany(FilterDefinition<Geolocation>.Empty);
        }

        [Fact]
        public void ThrowsExcepionWhenGettingGeolocationOnEmptyDb()
        {
            Assert.Throws<LocationApiEntityNotFoundException>(() => controller.Get("1.1.1.1").GetAwaiter().GetResult());
        }

        [Fact]
        public void Returns200WhenGeolocationExistsInDb()
        {
            var geolocation = new Geolocation()
            {
                City = "Warszawa",
                Continent = Enums.Continent.Africa,
                Country = "Poland",
                IP = "1.1.1.1",
                IPType = Enums.IPType.IPv4,
                Lat = 20,
                Lng = 21,
                Url = null
            };
            geolocationsCollection.InsertOne(geolocation);
            var actionResult = controller.Get("1.1.1.1").GetAwaiter().GetResult();
            Assert.IsType<OkNegotiatedContentResult<GeolocationViewModel>>(actionResult);
        }

        [Fact]
        public void Returns200WhenGeolocationExistsInDsb()
        {
            var geolocation = new Geolocation()
            {
                City = "Warszawa",
                Continent = Enums.Continent.Africa,
                Country = "Poland",
                IP = "1.1.1.1",
                IPType = Enums.IPType.IPv4,
                Lat = 20,
                Lng = 21,
                Url = null
            };
            geolocationsCollection.InsertOne(geolocation);

            var geolocationVM = new GeolocationViewModel()
            {
                City = "Warszawa",
                Continent = "Africa",
                Country = "Poland",
                IP = "1.1.1.1",
                IPType = "IPv4",
                Lat = 20,
                Lng = 21,
                Url = null
            };
            Assert.Throws<LocationApiDuplicateEntityException>(() => controller.Post(geolocationVM).GetAwaiter().GetResult());
        }
    }

}
