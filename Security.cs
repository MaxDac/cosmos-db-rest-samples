using System.Net;
using CosmosDbRestSamples.Types;
using Microsoft.Extensions.Logging;

namespace CosmosDbRestSamples;

public class Security
{
    private readonly ILogger<Security> m_logger;

    public Security(ILogger<Security> logger)
    {
        m_logger = logger;
    }
    
    public string GenerateMasterKeyAuthorizationSignature(HttpMethod verb, ResourceType resourceType, string resourceLink, string date, string key)
    {
        var keyType = "master";
        var tokenVersion = "1.0";
        var payload = $"{verb.ToString().ToLowerInvariant()}\n{resourceType.ToString().ToLowerInvariant()}\n{resourceLink}\n{date.ToLowerInvariant()}\n\n";

        var hmacSha256 = new System.Security.Cryptography.HMACSHA256 { Key = Convert.FromBase64String(key) };
        var hashPayload = hmacSha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(payload));
        var signature = Convert.ToBase64String(hashPayload);
        var authSet = WebUtility.UrlEncode($"type={keyType}&ver={tokenVersion}&sig={signature}");

        m_logger.LogInformation("Obtained security key: '{SecurityKey}'", authSet);

        return authSet;
    }
}