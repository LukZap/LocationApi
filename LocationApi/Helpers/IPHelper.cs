using LocationApi.Enums;
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
            ip = null;
            if (string.IsNullOrWhiteSpace(ipString))
                return false;

            if (ipString.Contains('.') || ipString.Contains(':'))
                return IPAddress.TryParse(ipString, out ip);

            return false;
        }

        public static bool TryResolveIPAddress(string hostName, out string ip, out IPType? ipType)
        {
            try
            {
                var address = Dns.GetHostEntry(hostName).AddressList.FirstOrDefault();

                switch (address.AddressFamily)
                {
                    case System.Net.Sockets.AddressFamily.InterNetwork:
                        ipType = IPType.IPv4;
                        break;
                    case System.Net.Sockets.AddressFamily.InterNetworkV6:
                        ipType = IPType.IPv6;
                        break;
                    default:
                        throw new Exception("Unsupported address family.");
                }

                ip = address.ToString();
                return true;
            }
            catch (Exception)
            {
                //log attempt to resolve ip
                ip = null;
                ipType = null;
                return false;
            }
        }

        public static bool TryResolveHostName(string ip, out string hostName)
        {
            try
            {
                var dnsEntry = Dns.GetHostEntry(ip);
                var hostNameArray = dnsEntry.HostName.Split('.');
                hostName = $"{hostNameArray[hostNameArray.Length - 2]}.{hostNameArray[hostNameArray.Length - 1]}";
                return true;
            }
            catch (Exception)
            {
                //log attempt to resolve hostname
                hostName = null;
                return false;
            }
        }
    }
}