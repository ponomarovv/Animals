using AspNetCoreRateLimit;

namespace Animals.API.Extensions;

public static class RateLimitExtensions
{
    public static IServiceCollection AddRateLimitHandler(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddMemoryCache();
        serviceCollection.Configure<IpRateLimitOptions>(options =>
        {
            options.EnableEndpointRateLimiting = true;
            options.StackBlockedRequests = false;
            options.HttpStatusCode = 429;
            options.RealIpHeader = "X-Real-IP";
            options.ClientIdHeader = "X-ClientId";
            options.GeneralRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Period = "10s",
                    Limit = 2,
                }
            };
        });
        serviceCollection.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        serviceCollection.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        serviceCollection.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        serviceCollection.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        serviceCollection.AddInMemoryRateLimiting();
        
        return serviceCollection;
    }
}
