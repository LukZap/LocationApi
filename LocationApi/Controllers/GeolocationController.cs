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
            if (result is null)
                return NotFound();
            return Ok(result);
        }

        public async Task<IHttpActionResult> Post([FromBody]GeolocationViewModel viewModel)
        {
            try
            {
                var result = await geolocationService.AddGeolocationAsync(viewModel);
                return Ok(result);
            }
            catch (Exception e) // exception filter?
            {
                return InternalServerError(e);
            }
        }

        public async Task<IHttpActionResult> Put([FromUri]string id,
            [FromBody]GeolocationViewModel viewModel)
        {
            if (id == null)
                return BadRequest("Id of geolocation to update cannot be null.");

            try
            {
                var result = await geolocationService.UpdateGeolocationAsync(id, viewModel);
                return Ok(result);
            }
            catch (Exception e)  // exception filter?
            {
                return InternalServerError(e);
            }
        }

        public async Task<IHttpActionResult> Delete([FromUri]string id)
        {
            if (id == null)
                return BadRequest("Id of geolocation to delete cannot be null.");

            try
            {
                await geolocationService.DeleteGeolocationAsync(id);
                return Ok(id);
            }
            catch (Exception e)  // exception filter?
            {
                return InternalServerError(e);
            }
        }
    }
}