namespace Animals.API.RateLimiting;

public class RateLimitingOptions
{
    public bool EnableEndpointRateLimiting { get; set; }
    public bool StackBlockedRequests { get; set; }
    public int HttpStatusCode { get; set; }
    public string RealIpHeader { get; set; }
    public string ClientIdHeader { get; set; }
    public List<AspNetCoreRateLimit.RateLimitRule> GeneralRules { get; set; }
}
