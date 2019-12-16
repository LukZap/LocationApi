using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace LocationApi.Exceptions
{
    public class LocationApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            var response = new HttpResponseMessage();

            if (context.Exception is LocationApiDuplicateEntityException)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            else if (context.Exception is LocationApiEntityNotFoundException)
            {
                response.StatusCode = HttpStatusCode.NotFound;

            }
            else if (context.Exception is ArgumentException)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }
            else
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }

            response.Content = new StringContent(context.Exception.Message);
            context.Response = response;
        }
    }
}