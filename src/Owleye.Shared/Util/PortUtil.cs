using DrHouse.Core;
using DrHouse.Telnet;
using System;
using System.Linq;

namespace Owleye.Shared.Util
{
    public static class PortUtil
    {
        static HealthChecker healthChecker;
        static PortUtil()
        {
            healthChecker = new HealthChecker("Owleye");
        }

        public static bool IsPortAlive(string hostFullAddress, int timeout)
        {
            try
            {
                var host = GetHostAddress(hostFullAddress);
                var port = GetPort(hostFullAddress);

                TelnetHealthDependency telnetDep = new TelnetHealthDependency(host, port.Value, hostFullAddress);
                healthChecker.AddDependency(telnetDep);

                HealthData health = healthChecker.CheckHealth();
                return health.IsOK;
            }
            catch
            {
                return false;
            }
        }

        private static string GetHostAddress(string fullAddress)
        {
            fullAddress=fullAddress.Trim();
            if (!fullAddress.Contains(":")) { return null; }
            return fullAddress.Split(':').First();
        }
        private static int? GetPort(string fullAddress)
        {
            fullAddress=fullAddress.Trim();
            if (!fullAddress.Contains(":")) { return null; }
            var splited = fullAddress.Split(':');
            return Convert.ToInt32(splited[1]);
        }
    }
}
