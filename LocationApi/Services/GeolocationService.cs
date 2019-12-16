using AutoMapper;
using LocationApi.Enums;
using LocationApi.Exceptions;
using LocationApi.Helpers;
using LocationApi.Models;
using LocationApi.Settings;
using LocationApi.ViewModels;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace LocationApi.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly IMapper mapper;
        private readonly IMongoCollection<Geolocation> geolocations;

        public GeolocationService(IMongoClient mongoClient, ILocationDbSettings locationDbSettings,
            IMapper mapper)
        {
            this.mapper = mapper;
            var mongoDatabase = mongoClient.GetDatabase(locationDbSettings.DatabaseName);
            geolocations = mongoDatabase.GetCollection<Geolocation>(locationDbSettings.GeolocationsCollection);
        }

        public async Task<GeolocationViewModel> GetGeolocationAsync(string locationQuery)
        {
            Geolocation geo;
            if (IPHelper.TryParseIPAddress(locationQuery, out IPAddress ip))
                geo = (await geolocations.FindAsync(g => g.IP == ip.ToString()))
                    .FirstOrDefault();
            else
                geo = (await geolocations.FindAsync(g => g.Url == locationQuery))
                    .FirstOrDefault();

            if (geo == null)
                throw new LocationApiEntityNotFoundException(typeof(Geolocation));

            var vm = mapper.Map<GeolocationViewModel>(geo);
            return vm;
        }

        public async Task<GeolocationViewModel> AddGeolocationAsync(GeolocationViewModel viewModel)
        {
            var geo = mapper.Map<Geolocation>(viewModel);

            await ValidateAndUpdateGeolocationBeforeDbAsync(geo);

            geo.Id = null;
            await geolocations.InsertOneAsync(geo);

            return mapper.Map<GeolocationViewModel>(geo);
        }

        public async Task<GeolocationViewModel> UpdateGeolocationAsync(string id, GeolocationViewModel viewModel)
        {
            var geo = (await geolocations.FindAsync(g => g.Id == id)).SingleOrDefault();
            if (geo == null)
                throw new LocationApiEntityNotFoundException(typeof(Geolocation), id);

            mapper.Map(viewModel, geo);

            await ValidateAndUpdateGeolocationBeforeDbAsync(geo);

            await geolocations.ReplaceOneAsync(g => g.Id == id, geo);

            return mapper.Map<GeolocationViewModel>(geo);
        }

        public async Task DeleteGeolocationAsync(string id)
        {
            if (!(await geolocations.FindAsync(g => g.Id == id)).Any())
                throw new LocationApiEntityNotFoundException(typeof(Geolocation), id);

            await geolocations.DeleteOneAsync(g => g.Id == id);
        }

        // write TESTS 
        private async Task ValidateAndUpdateGeolocationBeforeDbAsync(Geolocation geo)
        {
            if (!string.IsNullOrWhiteSpace(geo.IP))
            {
                var dbDuplicate = (await geolocations.FindAsync(g => g.IP == geo.IP && g.Id != geo.Id)).FirstOrDefault();
                if (dbDuplicate != null)
                    throw new LocationApiDuplicateEntityException(typeof(Geolocation), nameof(Geolocation.IP), dbDuplicate.IP);

                if (string.IsNullOrWhiteSpace(geo.Url) && IPHelper.TryResolveHostName(geo.IP, out string hostName))
                {
                    geo.Url = hostName;
                }
            }
            else if (!string.IsNullOrWhiteSpace(geo.Url))
            {
                if(IPHelper.TryResolveIPAddress(geo.Url, out string ip, out IPType? ipType))
                {
                    geo.IP = ip;
                    geo.IPType = ipType.Value;

                    var dbDuplicate = (await geolocations.FindAsync(g => g.IP == geo.IP && g.Id != geo.Id)).FirstOrDefault();
                    if (dbDuplicate != null)
                        throw new LocationApiDuplicateEntityException(typeof(Geolocation), nameof(Geolocation.IP), dbDuplicate.IP);
                }
            }
            else
                throw new ArgumentException("Cannot update or add geolocation without IP and Url");
        }
    }
}