using Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure
{
    public class MemoryCacheStore : ICache
    {
        private readonly IMemoryCache _cache;

        public MemoryCacheStore(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<T?> GetAsync<T>(string key, CancellationToken ct = default)
        {
            _cache.TryGetValue(key, out T? value);
            return Task.FromResult(value);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan ttl, CancellationToken ct = default)
        {
            _cache.Set(key, value, ttl);
            return Task.CompletedTask;
        }

        public Task RemoveAsync(string key, CancellationToken ct = default)
        {
            _cache.Remove(key);
            return Task.CompletedTask;
        }
    }
}