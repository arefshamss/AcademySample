using Academy.Application.Cache;
using Academy.Application.Services.Interfaces;
using EasyCaching.Core;

namespace Academy.Application.Services.Implementation;

public class MemoryCacheService(IEasyCachingProviderFactory cachingProviderFactory) : IMemoryCacheService
{
    private readonly IEasyCachingProvider _cachingProvider = cachingProviderFactory.GetCachingProvider(CacheProviders.InMemoryCachingProviderName);

    private readonly TimeSpan _defaultExpiration  = TimeSpan.FromDays(1);
    
    public Task<T> GetOrCreateAsync<T>(CacheKey key, Func<Task<T>> factory) 
    {
        return GetOrCreateAsync(key, factory, _defaultExpiration);
    }
    
    public async Task<T> GetOrCreateAsync<T>(CacheKey key , Func<Task<T>> factory, TimeSpan expiration) 
    {
        var cacheValue = await _cachingProvider.GetAsync<T>(key);
        
        if (cacheValue.HasValue) return cacheValue.Value;
        var newValue = await factory();
            
        await _cachingProvider.SetAsync(key, newValue, expiration);

        return newValue;
    }
    
    public async Task SetAsync<T>(CacheKey key, Func<Task<T>> factory) 
    {
        await SetAsync(key, factory, _defaultExpiration);
    }
    
    public async Task SetAsync<T>(CacheKey key, Func<Task<T>> factory, TimeSpan expiration) 
    {
        var value = await factory();
        await _cachingProvider.SetAsync(key, value, expiration);
    }

    public async Task RemoveAsync(CacheKey key)
        => await _cachingProvider.RemoveAsync(key);

    public async Task RemoveByPrefixAsync(CacheKey prefix)
        =>  await _cachingProvider.RemoveByPrefixAsync(prefix);
    

    public async Task RemoveByPatternAsync(CacheKey pattern)
        => await _cachingProvider.RemoveByPatternAsync(pattern);

    
    public async Task FlushAsync()
        => await _cachingProvider.FlushAsync();
    
}