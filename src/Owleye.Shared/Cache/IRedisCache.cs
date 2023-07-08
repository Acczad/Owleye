using System.Threading.Tasks;

namespace Owleye.Shared.Cache
{
    public interface IRedisCache
    {
        Task SetAsync<T>(string key, T objectToCache, int? expireTimeInSecond = null);
        Task<T> GetAsync<T>(string key);
        Task  Remove(string key);
    }
}
