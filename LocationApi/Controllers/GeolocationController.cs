using LocationApi.Services;
using LocationApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace LocationApi.Controllers
{
    public class GeolocationController : ApiController
    {
        private readonly IGeolocationService geolocationService;

        public GeolocationController(IGeolocationService geolocationService)
        {
            this.geolocationService = geolocationService;
        }

        public IHttpActionResult Get()
        {
            var result = geolocationService.GetGeolocations();
            return Ok( result );
        }

        public IHttpActionResult Get(int id)
        {
            return Ok(new Models.Geolocation() {  });
        }

        public void Post([FromBody]string value)
        {
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}