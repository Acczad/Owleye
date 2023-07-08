using System;
using System.Threading.Tasks;
using EasyCaching.Core;
using Microsoft.Extensions.Configuration;
using Owleye.Shared.Cache;
using Quartz.Impl.AdoJobStore.Common;

namespace Owleye.Infrastructure.Cache
{
    public class RedisCache : IRedisCache
    {
        private readonly IEasyCachingProviderFactory _distributedCache;
        private readonly IConfiguration _configuration;

        public RedisCache(
            IEasyCachingProviderFactory distributedCache,
            IConfiguration configuration)
        {
            _distributedCache = distributedCache;
            _configuration = configuration;
        }
        public async Task SetAsync<T>(string key, T objectToCache, int? expireTimeInSecond = null)
        {
            // TODO refactor this.
            var provider = _distributedCache.GetCachingProvider(_configuration["General:RedisInstanceName"]);

            if (!expireTimeInSecond.HasValue)
                await provider.SetAsync(key, objectToCache, TimeSpan.FromDays(120));

            else await provider.SetAsync(key, objectToCache, TimeSpan.FromSeconds(expireTimeInSecond.Value));

        }

        public async Task Remove(string key)
        {
            var provider = _distributedCache.GetCachingProvider(_configuration["General:RedisInstanceName"]);
            await provider.RemoveAsync(key);
        }

        public async Task<T> GetAsync<T>(string key)
        {
            // TODO refactor this.
            var provider = _distributedCache.GetCachingProvider(_configuration["General:RedisInstanceName"]);

            var cachedResult = await provider.GetAsync<T>(key);
            return cachedResult.Value;

        }
    }
}
