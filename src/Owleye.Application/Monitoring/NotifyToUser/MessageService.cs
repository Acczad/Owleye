using EnumsNET;
using Microsoft.Extensions.Caching.Memory;
using Owleye.Domain;
using Owleye.Shared.Extentions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Owleye.Application.Monitoring.NotifyToUser
{
    public interface IMessageService
    {
        bool IsMessageSent(string ServiceUrl, string ipAddress, SensorType sensorType, List<string> addresses, bool IsServiceAlive, string Name);
    }
    public class MessageService : IMessageService
    {
        private static readonly MemoryCache _cache = new MemoryCache(new MemoryCacheOptions());
        public bool IsMessageSent(string ServiceUrl, string ipAddress, SensorType sensorType, List<string> addresses, bool IsServiceAlive, string Name)
        {
            var strBuilder = new StringBuilder();
            strBuilder.AppendLine(ServiceUrl);
            strBuilder.AppendLine(ipAddress);
            strBuilder.AppendLine(sensorType.AsString(EnumFormat.Description));
            foreach (var mail in addresses)
            {
                strBuilder.Append(mail);
            }
            strBuilder.AppendLine(IsServiceAlive.ToString());
            strBuilder.AppendLine(Name);
            var messgeKey = strBuilder.ToString().CreateMD5Hash();
            if (string.IsNullOrEmpty(_cache.Get<string>(messgeKey)))
            {
                _cache.Set<string>(messgeKey, "sent", new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(3)));
                return false;
            }

            return true;
        }
    }
}
