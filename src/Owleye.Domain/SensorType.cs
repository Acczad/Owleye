using System.ComponentModel;

namespace Owleye.Domain
{
    public enum SensorType
    {
        [Description("Ping")]
        Ping = 0,
        DnsCheck = 1,
        [Description("HTTP Request GET")]
        HttpRequestGet = 2,
        [Description("PortCheck")]
        PortCheck = 3
    }
}