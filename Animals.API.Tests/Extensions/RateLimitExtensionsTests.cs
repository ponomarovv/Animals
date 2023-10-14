using Animals.API.Extensions;
using Animals.API.RateLimiting;
using AspNetCoreRateLimit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Animals.API.Tests.Extensions;

public class RateLimitExtensionsTests
{
    [Fact]
    public void AddRateLimitHandler_AddsServicesWithConfiguration()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("RateLimiting:EnableEndpointRateLimiting", "true"),
                new KeyValuePair<string, string>("RateLimiting:StackBlockedRequests", "true"),
                new KeyValuePair<string, string>("RateLimiting:HttpStatusCode", "429"),
                new KeyValuePair<string, string>("RateLimiting:RealIpHeader", "X-Real-IP"),
                new KeyValuePair<string, string>("RateLimiting:ClientIdHeader", "X-ClientId"),
                new KeyValuePair<string, string>("RateLimiting:GeneralRules:0:Endpoint", "api/rateLimited"),
                new KeyValuePair<string, string>("RateLimiting:GeneralRules:0:Limit", "1-M"),
                new KeyValuePair<string, string>("RateLimiting:GeneralRules:0:Period", "1s"),
            })
            .Build();

        // Act
        serviceCollection.AddRateLimitHandler(configuration);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var rateLimitOptions = serviceProvider.GetRequiredService<IOptions<IpRateLimitOptions>>().Value;

        // Assert
        Assert.NotNull(serviceProvider.GetRequiredService<IIpPolicyStore>());
        Assert.NotNull(serviceProvider.GetRequiredService<IRateLimitCounterStore>());
        Assert.NotNull(serviceProvider.GetRequiredService<IRateLimitConfiguration>());
        Assert.NotNull(serviceProvider.GetRequiredService<IProcessingStrategy>());
        Assert.True(rateLimitOptions.EnableEndpointRateLimiting);
        Assert.True(rateLimitOptions.StackBlockedRequests);
        Assert.Equal(429, rateLimitOptions.HttpStatusCode);
        Assert.Equal("X-Real-IP", rateLimitOptions.RealIpHeader);
        Assert.Equal("X-ClientId", rateLimitOptions.ClientIdHeader);
        
    }
}
