using LocationApi.Test.Fixtures;
using LocationApi.Validators;
using LocationApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LocationApi.Test
{
    public class GeolocationViewModelValidatorTests : IClassFixture<GeolocationViewModelValidatorFixture>
    {
        private readonly GeolocationViewModelValidator validator;

        public GeolocationViewModelValidatorTests(GeolocationViewModelValidatorFixture validatorFixture)
        {
            validator = validatorFixture.Validator;
        }

        [Fact]
        public void ReturnsNoErrorsWhenValidatesCorrectGeolocationViewModelWithNoUrl()
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
            var validationResult = validator.Validate(geolocationVM);
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void ReturnsNoErrorsWhenValidatesCorrectGeolocationViewModelWithNoUrlAndIPv6()
        {
            var geolocationVM = new GeolocationViewModel()
            {
                City = "Warszawa",
                Continent = "Africa",
                Country = "Poland",
                IP = "2a02:a317:2140:e980:d568:2f1e:d473:20d",
                IPType = "IPv6",
                Lat = 20,
                Lng = 21,
                Url = null
            };
            var validationResult = validator.Validate(geolocationVM);
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void ReturnsNoErrorsWhenValidatesCorrectGeolocationViewModelWithNoIP()
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
            var validationResult = validator.Validate(geolocationVM);
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void ReturnsErrorsWhenValidatesGeolocationViewModelWithNoIPAndUrl()
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
            var validationResult = validator.Validate(geolocationVM);
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public void ReturnsErrorsWhenValidatesGeolocationViewModelWithIPButNoIPType()
        {
            var geolocationVM = new GeolocationViewModel()
            {
                City = "Warszawa",
                Continent = "Africa",
                Country = "Poland",
                IP = "1.1.1.1",
                IPType = null,
                Lat = 20,
                Lng = 21,
                Url = null
            };
            var validationResult = validator.Validate(geolocationVM);
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public void ReturnsErrorsWhenValidatesGeolocationViewModelWithNoCity()
        {
            var geolocationVM = new GeolocationViewModel()
            {
                City = null,
                Continent = "Africa",
                Country = "Poland",
                IP = "1.1.1.1",
                IPType = "IPv4",
                Lat = 20,
                Lng = 21,
                Url = null
            };
            var validationResult = validator.Validate(geolocationVM);
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public void ReturnsErrorsWhenValidatesGeolocationViewModelWithNoCountry()
        {
            var geolocationVM = new GeolocationViewModel()
            {
                City = "Warszawa",
                Continent = "Africa",
                Country = null,
                IP = "1.1.1.1",
                IPType = "IPv4",
                Lat = 20,
                Lng = 21,
                Url = null
            };
            var validationResult = validator.Validate(geolocationVM);
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public void ReturnsErrorsWhenValidatesGeolocationViewModelWithNoContinent()
        {
            var geolocationVM = new GeolocationViewModel()
            {
                City = "Warszawa",
                Continent = null,
                Country = "Poland",
                IP = "1.1.1.1",
                IPType = "IPv4",
                Lat = 20,
                Lng = 21,
                Url = null
            };
            var validationResult = validator.Validate(geolocationVM);
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public void ReturnsErrorsWhenValidatesGeolocationViewModelWithInvalidContinent()
        {
            var geolocationVM = new GeolocationViewModel()
            {
                City = "Warszawa",
                Continent = "Atlantyda",
                Country = "Poland",
                IP = "1.1.1.1",
                IPType = "IPv4",
                Lat = 20,
                Lng = 21,
                Url = null
            };
            var validationResult = validator.Validate(geolocationVM);
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public void ReturnsErrorsWhenValidatesGeolocationViewModelWithInvalidIPType()
        {
            var geolocationVM = new GeolocationViewModel()
            {
                City = "Warszawa",
                Continent = "Africa",
                Country = "Poland",
                IP = "1.1.1.1",
                IPType = "some.invalid.value",
                Lat = 20,
                Lng = 21,
                Url = null
            };
            var validationResult = validator.Validate(geolocationVM);
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public void ReturnsErrorsWhenValidatesGeolocationViewModelWithInvalidIP()
        {
            var geolocationVM = new GeolocationViewModel()
            {
                City = "Warszawa",
                Continent = "Africa",
                Country = "Poland",
                IP = "1.1.1.1.invalid.value",
                IPType = "IPv4",
                Lat = 20,
                Lng = 21,
                Url = null
            };
            var validationResult = validator.Validate(geolocationVM);
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public void ReturnsErrorsWhenValidatesGeolocationViewModelWithInvalidLat()
        {
            var geolocationVM = new GeolocationViewModel()
            {
                City = "Warszawa",
                Continent = "Africa",
                Country = "Poland",
                IP = "1.1.1.1",
                IPType = "IPv4",
                Lat = 200,
                Lng = 21,
                Url = null
            };
            var validationResult = validator.Validate(geolocationVM);
            Assert.False(validationResult.IsValid);
        }


        [Fact]
        public void ReturnsErrorsWhenValidatesGeolocationViewModelWithInvalidLng()
        {
            var geolocationVM = new GeolocationViewModel()
            {
                City = "Warszawa",
                Continent = "Africa",
                Country = "Poland",
                IP = "1.1.1.1",
                IPType = "IPv4",
                Lat = 20,
                Lng = -210,
                Url = null
            };
            var validationResult = validator.Validate(geolocationVM);
            Assert.False(validationResult.IsValid);
        }
    }
}
