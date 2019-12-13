using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationApi.Settings
{
    public class LocationDbSettings : ILocationDbSettings
    {
        public string GeolocationsCollection { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}