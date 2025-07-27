﻿namespace SurveyBasket.Services.Caches
{

    // This service when i use Distributed cache 
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string cacheKey, CancellationToken cancellationToken = default) where T : class;
        Task SetAsync<T>(string cacheKey, T value, CancellationToken cancellationToken = default) where T : class;
        Task RemoveAsync(string cacheKey, CancellationToken cancellationToken = default);
    }
}
