using FluentValidation.Attributes;
using LocationApi.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationApi.ViewModels
{
    [Validator(typeof(GeolocationViewModelValidator))]
    public class GeolocationViewModel
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public string IP { get; set; }

        public string IPType { get; set; }

        public string Continent { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }
    }
}