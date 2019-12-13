using LocationApi.Models;
using LocationApi.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationApi.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly IMongoDatabase mongoDatabase;
        private readonly ILocationDbSettings locationDbSettings;
        private readonly IMongoCollection<Geolocation> geolocations;

        public GeolocationService(IMongoDatabase mongoDatabase, ILocationDbSettings locationDbSettings)
        {
            this.mongoDatabase = mongoDatabase;
            this.locationDbSettings = locationDbSettings;
            geolocations = mongoDatabase.GetCollection<Geolocation>(locationDbSettings.GeolocationsCollection);
        }

        public IEnumerable<Geolocation> GetGeolocations()
        {
            var geos = geolocations.Find(book => true).ToList();
            return geos;
        }
    }
}