using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace LocationApi.Helpers
{
    public static class IPHelper
    {
        public static bool TryParseIPAddress(string ipString, out IPAddress ip)
        {
            if (ipString.Contains('.') || ipString.Contains(':'))
                return IPAddress.TryParse(ipString, out ip);
            else
                ip = null;
            return false;
        }
    }
}