using Core.Abstractions;
using Core.Settings;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;


namespace Core.Behaviors
{
    // CacheBehavior class implements IPipelineBehavior<TRequest, TResponse> where the request is of type ICacheableMediatrQuery.
    // This means that caching will be applicable only to the MediatR handlers that are of type ICacheableMediatrQuery. Pretty handy.
    public class CachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ICacheableQuery
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger _logger;
        private readonly CacheSettings _settings;

        public CachingBehavior(IDistributedCache cache, ILogger<TResponse> logger, IOptions<CacheSettings> settings)
        {
            _cache = cache;
            _logger = logger;
            _settings = settings.Value;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            TResponse response;
            
            //If the BypassCache property is set to true, the behavior goes to the next request/response and avoids caching.
            if (request.BypassCache) return await next();

            async Task<TResponse> GetResponseAndAddToCache()
            {
                //waits for the response from the server/database.
                response = await next();
                
                // creates an expiration timespan in hours, either from the appsettings or the value passed from the mediatR handler. Now, this is the best part.
                // Each mediatR handler can decide how long a cache has to be available in the cache store.
                var slidingExpiration = request.SlidingExpiration ?? TimeSpan.FromHours(_settings.SlidingExpiration);
                
                var options = new DistributedCacheEntryOptions { SlidingExpiration = slidingExpiration };
                var serializedData = Encoding.Default.GetBytes(JsonConvert.SerializeObject(response));
                await _cache.SetAsync(request.CacheKey, serializedData, options, cancellationToken);
                return response;
            }
            
            //Fetches from IDistributedCache instance and checks if any data with the passed cache key exists.
            var cachedResponse = await _cache.GetAsync(request.CacheKey, cancellationToken);
            
            //if the cache exists, then returns from the cache-store and logs ‘Fetched from cache’
            if (cachedResponse != null)
            {
                response = JsonConvert.DeserializeObject<TResponse>(Encoding.Default.GetString(cachedResponse));
                _logger.LogInformation($"Fetched from Cache -> '{request.CacheKey}'.");
            }
            else
            {
                response = await GetResponseAndAddToCache();
                _logger.LogInformation($"Added to Cache -> '{request.CacheKey}'.");
            }

            return response;
        }
    }
}