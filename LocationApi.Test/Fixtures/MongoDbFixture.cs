using LocationApi.Models;
using Mongo2Go;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationApi.Test.Fixtures
{
    public class MongoDbFixture : IDisposable
    {
        public IMongoCollection<Geolocation> Collection { get; private set; }
        public IMongoClient MongoClient { get; private set; }

        private readonly MongoDbRunner _runner;
        private readonly string _databaseName = "IntegrationTest";
        private readonly string _collectionName = "TestGeolocations";

        public MongoDbFixture()
        {
            _runner = MongoDbRunner.Start(singleNodeReplSet: false);

            MongoClient = new MongoClient(_runner.ConnectionString);
            IMongoDatabase database = MongoClient.GetDatabase(_databaseName);
            Collection = database.GetCollection<Geolocation>(_collectionName);

            var geolocationBuilder = Builders<Geolocation>.IndexKeys;
            var indexModel = new CreateIndexModel<Geolocation>(geolocationBuilder.Ascending(x => x.IP),
                new CreateIndexOptions { Unique = true });
            Collection.Indexes.CreateOne(indexModel);

            var jsonCommandText = File.ReadAllText("./mongoDbSchemaCommand.json");
            database.RunCommand(new JsonCommand<object>(jsonCommandText));
        }

        public void Dispose()
        {
            _runner.Dispose();
        }
    }
}
