using LocationApi.Models;
using LocationApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationApi.Services
{
    public interface IGeolocationService
    {
        Task<GeolocationViewModel> GetGeolocationAsync(string locationQuery);
        Task<GeolocationViewModel> AddGeolocationAsync(GeolocationViewModel viewModel);
        Task<GeolocationViewModel> UpdateGeolocationAsync(string id, GeolocationViewModel viewModel);
        Task DeleteGeolocationAsync(string id);
    }
}
