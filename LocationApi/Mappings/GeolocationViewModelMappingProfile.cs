using AutoMapper;
using LocationApi.Models;
using LocationApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationApi.Mappings
{
    public class GeolocationViewModelMappingProfile : Profile
    {
        public GeolocationViewModelMappingProfile()
        {
            CreateMap<Geolocation, GeolocationViewModel>();
            CreateMap<GeolocationViewModel, Geolocation>()
                .ForMember(x => x.Url, x => x.MapFrom((s,d) => {
                    if (string.IsNullOrWhiteSpace(s.Url))
                        return null;
                    var hostNameArray = s.Url.Split('.');
                    return $"{hostNameArray[hostNameArray.Length - 2]}.{hostNameArray[hostNameArray.Length - 1]}";
                }))
                .ForAllMembers(o => o.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}