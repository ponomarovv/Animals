using Animals.API.RateLimiting;
using AspNetCoreRateLimit;
using RateLimitRule = AspNetCoreRateLimit.RateLimitRule;

namespace Animals.API.Extensions;

public static class RateLimitExtensions
{
    public static IServiceCollection AddRateLimitHandler(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        
        var rateLimitingOptions = configuration.GetSection("RateLimiting").Get<RateLimitingOptions>();
        
        serviceCollection.AddMemoryCache();
        serviceCollection.Configure<IpRateLimitOptions>(options =>
        {
            options.EnableEndpointRateLimiting = rateLimitingOptions.EnableEndpointRateLimiting;
            options.StackBlockedRequests = rateLimitingOptions.StackBlockedRequests;
            options.HttpStatusCode = rateLimitingOptions.HttpStatusCode;
            options.RealIpHeader = rateLimitingOptions.RealIpHeader;
            options.ClientIdHeader = rateLimitingOptions.ClientIdHeader;
            options.GeneralRules = rateLimitingOptions.GeneralRules;
        });

        serviceCollection.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        serviceCollection.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        serviceCollection.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        serviceCollection.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        serviceCollection.AddInMemoryRateLimiting();
        
        return serviceCollection;
    }
}
