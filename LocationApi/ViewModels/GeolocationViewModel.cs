using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationApi.ViewModels
{
    public class GeolocationViewModel
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public string IP { get; set; }

        public string IPType { get; set; }

        public string Continent { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public decimal Lat { get; set; }

        public decimal Lng { get; set; }
    }
}