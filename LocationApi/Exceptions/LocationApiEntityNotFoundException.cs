using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationApi.Exceptions
{
    public class LocationApiEntityNotFoundException : Exception
    {
        public LocationApiEntityNotFoundException(Type type, string id)
            : base($"Entity not found - type: {type.Name}, ID: {id}")
        {

        }

        public LocationApiEntityNotFoundException(Type type)
            : base($"Entity not found - type: {type.Name}")
        {

        }
    }
}