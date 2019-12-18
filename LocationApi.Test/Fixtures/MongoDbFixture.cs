using LocationApi.Models;
using LocationApi.Settings;
using Mongo2Go;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationApi.Test.Fixtures
{
    public class MongoDbFixture : IDisposable
    {
        public IMongoCollection<Geolocation> Collection { get; }
        public IMongoClient MongoClient { get; }
        public ILocationDbSettings MongoDbSettings { get; }

        private readonly MongoDbRunner runner;

        public MongoDbFixture()
        {
            runner = MongoDbRunner.Start(singleNodeReplSet: false);
            
            MongoDbSettings = CreateLocationDbSettings();
            MongoDbSettings.ConnectionString = runner.ConnectionString;

            MongoClient = new MongoClient(runner.ConnectionString);
            IMongoDatabase database = MongoClient.GetDatabase(MongoDbSettings.DatabaseName);
            Collection = database.GetCollection<Geolocation>(MongoDbSettings.GeolocationsCollection);

            var geolocationBuilder = Builders<Geolocation>.IndexKeys;
            var indexModel = new CreateIndexModel<Geolocation>(geolocationBuilder.Ascending(x => x.IP),
                new CreateIndexOptions { Unique = true });
            Collection.Indexes.CreateOne(indexModel);

            var jsonCommandText = File.ReadAllText("./mongoDbSchemaCommand.json");
            database.RunCommand(new JsonCommand<object>(jsonCommandText));
        }

        public void Dispose()
        {
            runner.Dispose();
        }

        private ILocationDbSettings CreateLocationDbSettings()
        {
            return new LocationDbSettings
            {
                GeolocationsCollection = ConfigurationManager.AppSettings["GeolocationsCollection"],
                DatabaseName = ConfigurationManager.AppSettings["DatabaseName"]
            };
        }
    }
}
