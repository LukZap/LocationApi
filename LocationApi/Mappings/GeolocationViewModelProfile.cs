using AutoMapper;
using LocationApi.Models;
using LocationApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationApi.Mappings
{
    public class GeolocationViewModelProfile : Profile
    {
        public GeolocationViewModelProfile()
        {
            CreateMap<Geolocation, GeolocationViewModel>();
            CreateMap<GeolocationViewModel, Geolocation>();
        }
    }
}