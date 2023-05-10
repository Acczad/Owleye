using System;
using System.Linq;
using System.Net.Sockets;

namespace Owleye.Shared.Util
{
    public static class PortUtil
    {
        public static bool IsPortAlive(string hostFullAddress, int timeout)
        {
            try
            {
                var host = GetHostAddress(hostFullAddress);
                var port = GetPort(hostFullAddress);
                if (host == null || port==null) return false;

                using (var client = new TcpClient())
                {
                    var result = client.BeginConnect(host, port.Value, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(timeout);
                    client.EndConnect(result);
                    return success;
                }
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
