using AutoMapper;
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
    // write Exceptions for ie. duplicates in db etc.
    public class GeolocationService : IGeolocationService
    {
        private readonly IMongoDatabase mongoDatabase;
        private readonly ILocationDbSettings locationDbSettings;
        private readonly IMapper mapper;
        private readonly IMongoCollection<Geolocation> geolocations;

        public GeolocationService(IMongoDatabase mongoDatabase, ILocationDbSettings locationDbSettings,
            IMapper mapper)
        {
            this.mongoDatabase = mongoDatabase;
            this.locationDbSettings = locationDbSettings;
            this.mapper = mapper;
            geolocations = mongoDatabase.GetCollection<Geolocation>(locationDbSettings.GeolocationsCollection);
        }

        public async Task<GeolocationViewModel> GetGeolocationAsync(string locationQuery)
        {
            Geolocation geo;
            if (IPHelper.TryParseIPAddress(locationQuery, out IPAddress ip))
                geo = (await geolocations.FindAsync(g => g.IP == ip.ToString()))
                    .FirstOrDefault();
            else
            {
                // find by authority for the Url
                geo = (await geolocations.FindAsync(g => g.Url == locationQuery))
                    .FirstOrDefault();
            }

            var vm = mapper.Map<GeolocationViewModel>(geo);
            return vm;
        }

        public async Task<GeolocationViewModel> AddGeolocationAsync(GeolocationViewModel viewModel)
        {
            var geo = mapper.Map<Geolocation>(viewModel); // mapping and normalization

            await ValidateAndUpdateGeolocationBeforeDbAsync(geo);

            geo.Id = null;
            await geolocations.InsertOneAsync(geo);  // mongo doc validation?
            return mapper.Map<GeolocationViewModel>(viewModel);
        }

        public async Task<GeolocationViewModel> UpdateGeolocationAsync(string id, GeolocationViewModel viewModel)
        {
            if (!geolocations.Find(g => g.Id == id).Any())
                throw new Exception(); // no location was found to update
            var geo = mapper.Map<Geolocation>(viewModel); // mapping and normalization

            await ValidateAndUpdateGeolocationBeforeDbAsync(geo);

            geo.Id = id;
            await geolocations.ReplaceOneAsync(g => g.Id == id, geo);
            return mapper.Map<GeolocationViewModel>(viewModel);
        }

        public async Task DeleteGeolocationAsync(string id)
        {
            if (!geolocations.Find(g => g.Id == id).Any())
                throw new Exception(); // no location was found to update

            await geolocations.DeleteOneAsync(g => g.Id == id);
        }

        // write TEST 
        private async Task ValidateAndUpdateGeolocationBeforeDbAsync(Geolocation geo)
        {
            if (!string.IsNullOrWhiteSpace(geo.IP))
            {
                if ((await geolocations.FindAsync(g => g.IP == geo.IP)).Any())
                    throw new Exception(); // duplicate

                if (string.IsNullOrWhiteSpace(geo.Url))
                {
                    var hostName = Dns.GetHostEntry(geo.IP).HostName;
                    if (hostName != null) // try resolve ip address
                        geo.Url = new Uri(hostName).Authority;
                }
                else
                {
                    if ((await geolocations.FindAsync(g => g.IP == geo.IP)).Any())
                        throw new Exception(); // duplicate
                }
            }
            else if (!string.IsNullOrWhiteSpace(geo.Url))
            {
                if ((await geolocations.FindAsync(g => g.Url == geo.Url)).Any())
                    throw new Exception(); // duplicate

                var addresses = Dns.GetHostEntry(geo.Url).AddressList;
                var address = addresses?.FirstOrDefault();
                if (address == null)
                    throw new Exception(); // can't resolve ip address

                switch (address.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        geo.IPType = Enums.IPType.IPv4;
                        break;
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        geo.IPType = Enums.IPType.IPv6;
                        break;
                    default:
                        throw new Exception(); // wrong address familly
                }

                if ((await geolocations.FindAsync(g => g.IP == address.ToString())).Any())
                    throw new Exception(); // duplicate

                geo.IP = address.ToString();
            }
            else
                throw new Exception(); // can't add geolocation without ip and url
        }
    }
}