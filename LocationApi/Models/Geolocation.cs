using LocationApi.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationApi.Models
{
    public class Geolocation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Url { get; set; }

        public string IP { get; set; } 

        public IPType? IPType { get; set; }

        public Continent Continent { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public decimal Lat { get; set; }

        public decimal Lng { get; set; }
    }
}