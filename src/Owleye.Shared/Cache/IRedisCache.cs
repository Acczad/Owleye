using System.Threading.Tasks;

namespace Owleye.Shared.Cache
{
    public interface IRedisCache
    {
        Task SetAsync<T>(string key, T objectToCache);
        Task<T> GetAsync<T>(string key);
        Task  Remove(string key);
    }
}
