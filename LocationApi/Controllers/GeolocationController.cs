using LocationApi.Exceptions;
using LocationApi.Services;
using LocationApi.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace LocationApi.Controllers
{
    [LocationApiExceptionFilter]
    public class GeolocationController : ApiController
    {
        private readonly IGeolocationService geolocationService;

        public GeolocationController(IGeolocationService geolocationService)
        {
            this.geolocationService = geolocationService;
        }

        public async Task<IHttpActionResult> Get([FromUri]string q)
        {
            var result = await geolocationService.GetGeolocationAsync(q);

            return Ok(result);
        }

        public async Task<IHttpActionResult> Post([FromBody]GeolocationViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await geolocationService.AddGeolocationAsync(viewModel);
            return Ok(result);
        }

        public async Task<IHttpActionResult> Put([FromUri]string id,
            [FromBody]GeolocationViewModel viewModel)
        {
            if (id is null)
                return BadRequest("Id of geolocation to update cannot be null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await geolocationService.UpdateGeolocationAsync(id, viewModel);
            return Ok(result);
        }

        public async Task<IHttpActionResult> Delete([FromUri]string id)
        {
            if (id is null)
                return BadRequest("Id of geolocation to delete cannot be null.");

            await geolocationService.DeleteGeolocationAsync(id);
            return Ok(id);
        }
    }
}