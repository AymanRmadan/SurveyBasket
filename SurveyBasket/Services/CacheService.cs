
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace SurveyBasket.Services
{
    public class CacheService(IDistributedCache distributedCache) : ICacheService
    {
        private readonly IDistributedCache _distributedCache = distributedCache;

        public async Task<T?> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default) where T : class
        {
            var cachedValue = await _distributedCache.GetStringAsync(cacheKey, cancellationToken);

            return String.IsNullOrEmpty(cachedValue)
                ? null
                : JsonSerializer.Deserialize<T>(cachedValue);
        }


        public async Task SetAsync<T>(string cacheKey, T value, CancellationToken cancellationToken = default) where T : class
        {
            // JsonSerializer.Serialize(value) ==> convert value to string by do serializr
            await _distributedCache.SetStringAsync(cacheKey, JsonSerializer.Serialize(value), cancellationToken);
        }

        public async Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default)
        {
            await _distributedCache.RemoveAsync(cacheKey, cancellationToken);
        }
    }
}
