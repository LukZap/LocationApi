using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LocationApi.Exceptions
{
    public class LocationApiDuplicateEntityException : Exception
    {
        public LocationApiDuplicateEntityException(Type type, string columnName, object value)
            : base($"Duplicate entity - type: {type.Name}, column name: {columnName}, value: {value}")
        {

        }
    }
}