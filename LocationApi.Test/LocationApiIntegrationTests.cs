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
        public void ThrowsExcepionWhenQueryStringNotMatchingAnyEntityInDb()
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
            Assert.Throws<LocationApiEntityNotFoundException>(() => controller.Get("2.2.2.2").GetAwaiter().GetResult());
        }

        [Fact]
        public void ThrowsExcepionWhenAddingGeolocationWithDuplicateIPToDb()
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

        [Fact]
        public void Return200WhenAddingGeolocationWithDuplicateURLToDb()
        {
            // assuming that one URL can point to many IP addresses
            var geolocation = new Geolocation()
            {
                City = "Warszawa",
                Continent = Enums.Continent.Africa,
                Country = "Poland",
                IP = "1.1.1.1",
                IPType = Enums.IPType.IPv4,
                Lat = 20,
                Lng = 21,
                Url = "onet.pl"
            };
            geolocationsCollection.InsertOne(geolocation);

            var geolocationVM = new GeolocationViewModel()
            {
                City = "Warszawa",
                Continent = "Africa",
                Country = "Poland",
                IP = "2.2.2.2",
                IPType = "IPv4",
                Lat = 20,
                Lng = 21,
                Url = "onet.pl"
            };
            var actionResult = controller.Post(geolocationVM).GetAwaiter().GetResult();
            Assert.IsType<OkNegotiatedContentResult<GeolocationViewModel>>(actionResult);
        }

        [Fact]
        public void Returns200WhenAddingValidGeolocationToDb()
        {
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
            var actionResult = controller.Post(geolocationVM).GetAwaiter().GetResult();
            Assert.IsType<OkNegotiatedContentResult<GeolocationViewModel>>(actionResult);
        }

        [Fact]
        public void ReturnsEntityWithNewIDAndResolvedURLWhenAddingValidGeolocationWithEmptyURLToDb()
        {
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
            var actionResult = controller.Post(geolocationVM).GetAwaiter().GetResult();
            var okResult = Assert.IsType<OkNegotiatedContentResult<GeolocationViewModel>>(actionResult);
            var newGeolocaion = okResult.Content;
            Assert.NotNull(newGeolocaion.Id);
            Assert.NotNull(newGeolocaion.Url);
        }

        [Fact]
        public void ReturnsEntityWithNewIDAndResolvedIPWhenAddingValidGeolocationWithEmptyIPToDb()
        {
            var geolocationVM = new GeolocationViewModel()
            {
                City = "Warszawa",
                Continent = "Africa",
                Country = "Poland",
                IP = null,
                IPType = null,
                Lat = 20,
                Lng = 21,
                Url = "onet.pl"
            };
            var actionResult = controller.Post(geolocationVM).GetAwaiter().GetResult();
            var okResult = Assert.IsType<OkNegotiatedContentResult<GeolocationViewModel>>(actionResult);
            var newGeolocaion = okResult.Content;
            Assert.NotNull(newGeolocaion.Id);
            Assert.NotNull(newGeolocaion.IP);
            Assert.NotNull(newGeolocaion.IPType);
        }

        [Fact]
        public void ReturnsEntityWithTruncatedURLWhenAddingValidGeolocationWithLongURLToDb()
        {
            var geolocationVM = new GeolocationViewModel()
            {
                City = "Warszawa",
                Continent = "Africa",
                Country = "Poland",
                IP = null,
                IPType = null,
                Lat = 20,
                Lng = 21,
                Url = "something.very.long.here.onet.pl"
            };
            var actionResult = controller.Post(geolocationVM).GetAwaiter().GetResult();
            var okResult = Assert.IsType<OkNegotiatedContentResult<GeolocationViewModel>>(actionResult);
            var newGeolocaion = okResult.Content;
            Assert.Equal("onet.pl", newGeolocaion.Url);
        }

        [Fact]
        public void ThrowsExcepionWhenAddingGeolocationWithEmptyURLAndIPToDb()
        {
            var geolocationVM = new GeolocationViewModel()
            {
                City = "Warszawa",
                Continent = "Africa",
                Country = "Poland",
                IP = null,
                IPType = null,
                Lat = 20,
                Lng = 21,
                Url = null
            };
            Assert.Throws<ArgumentException>(() => controller.Post(geolocationVM).GetAwaiter().GetResult());
        }

        [Fact]
        public void ThrowsExcepionWhenAddingGeolocationWithEmptyCityToDb()
        {
            var geolocationVM = new GeolocationViewModel()
            {
                City = null,
                Continent = "Africa",
                Country = "Poland",
                IP = null,
                IPType = null,
                Lat = 20,
                Lng = 21,
                Url = "onet.pl"
            };
            Assert.Throws<MongoWriteException>(() => controller.Post(geolocationVM).GetAwaiter().GetResult());
        }

        [Fact]
        public void ThrowsExcepionWhenAddingGeolocationWithURLThatCannotBeResolvedToIPToDb()
        {
            // assuming IP address is more important to location and we cannot depend on URL as it can point to many IPs
            var geolocationVM = new GeolocationViewModel()
            {
                City = "Warszawa",
                Continent = "Africa",
                Country = "Poland",
                IP = null,
                IPType = null,
                Lat = 20,
                Lng = 21,
                Url = "somenotexistingcrazyurl.pl"
            };
            Assert.Throws<MongoWriteException>(() => controller.Post(geolocationVM).GetAwaiter().GetResult());
        }

        [Fact]
        public void Returns400WhenUpdatingGeolocationWithoutPassingID()
        {
            var geolocationVM = new GeolocationViewModel()
            {
                City = "Warszawa",
                Continent = "Africa",
                Country = "Poland",
                IP = null,
                IPType = null,
                Lat = 20,
                Lng = 21,
                Url = "somenotexistingcrazyurl.pl"
            };
            Assert.IsType<BadRequestErrorMessageResult>(controller.Put(null, geolocationVM).GetAwaiter().GetResult());
        }

        //TODO write some update requests test - similar to add

        [Fact]
        public void Returns400WhenDeletingGeolocationWithoutPassingID()
        {
            Assert.IsType<BadRequestErrorMessageResult>(controller.Delete(null).GetAwaiter().GetResult());
        }

        [Fact]
        public void ThrowsExcepionWhenDeletingNonExistingGeolocation()
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
            var id = geolocation.Id;
            var arr = id.ToCharArray();
            arr[0] = arr[0] == '1' ? arr[0] = '2' : arr[0] = '1'; // change id
            id = new string(arr);

            Assert.Throws<LocationApiEntityNotFoundException>(() => controller.Delete(id).GetAwaiter().GetResult());
        }

        [Fact]
        public void Returns200WhenDeletingGeolocation()
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
            var id = geolocation.Id;

            Assert.IsType<OkNegotiatedContentResult<string>>(controller.Delete(id).GetAwaiter().GetResult());
        }

        [Fact]
        public void ReturnsWhenDeletingGeolocation()
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
            var id = geolocation.Id;

            controller.Delete(id).GetAwaiter().GetResult();

            Assert.Throws<LocationApiEntityNotFoundException>(() => controller.Get("1.1.1.1").GetAwaiter().GetResult());
        }
    }
}
