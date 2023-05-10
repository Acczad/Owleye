using System.ComponentModel;

namespace Owleye.Domain
{
    public enum SensorType
    {
        [Description("Ping")]
        Ping = 0,
        DnsCheck = 1,
        [Description("PageLoad")]
        PageLoad = 2,
        [Description("PortCheck")]
        PortCheck = 3
    }
}