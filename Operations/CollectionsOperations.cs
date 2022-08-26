using CosmosDbRestSamples.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CosmosDbRestSamples.Operations;

public class CollectionsOperations
{
    private readonly IOptions<Configuration> m_configuration;
    private readonly Security m_security;
    private readonly HttpClient m_client;
    private readonly OutputReporter m_reporter;
    private readonly ILogger<CollectionsOperations> m_logger;

    public CollectionsOperations(
        IOptions<Configuration> configuration, 
        Security security, 
        HttpClient client, 
        OutputReporter reporter,
        ILogger<CollectionsOperations> logger)
    {
        m_configuration = configuration;
        m_security = security;
        m_client = client;
        m_reporter = reporter;
        m_logger = logger;
    }

    public async Task CreateContainer(string databaseId, string containerId, DatabaseThoughputMode mode)
    {
        var method = HttpMethod.Post;

        var resourceType = ResourceType.colls;
        var resourceLink = $"dbs/{databaseId}";
        var requestDateString = "fri, 26 aug 2022 23:24:58 gmt";
        var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString, m_configuration.Value.Key);

        m_client.DefaultRequestHeaders.Clear();
        m_client.DefaultRequestHeaders.Add("Accept", "application/json");
        m_client.DefaultRequestHeaders.Add("authorization", auth);
        m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
        m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");

        if (mode == DatabaseThoughputMode.@fixed)
            m_client.DefaultRequestHeaders.Add("x-ms-offer-throughput", "400");
        if (mode == DatabaseThoughputMode.autopilot)
            m_client.DefaultRequestHeaders.Add("x-ms-cosmos-offer-autopilot-settings", "{\"maxThroughput\": 4000}");

        var requestUri = new Uri($"{m_configuration.Value.Host}/{resourceLink}/colls");
        var requestBody = $@"{{
    ""id"":""{containerId}"",
    ""partitionKey"": {{  
        ""paths"": [
        ""/pk""  
        ],  
        ""kind"": ""Hash"",
        ""Version"": 2
    }}  
    }}";
        var requestContent = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");

        var httpRequest = new HttpRequestMessage { Method = method, Content = requestContent, RequestUri = requestUri };

        var httpResponse = await m_client.SendAsync(httpRequest);
        await m_reporter.ReportOutput($"Create Container with thoughput mode {mode}:", httpResponse);
    }

    public async Task GetContainer(string databaseId, string containerId)
    {
        var method = HttpMethod.Get;

        var resourceType = ResourceType.colls;
        var resourceLink = $"dbs/{databaseId}/colls/{containerId}";
        var requestDateString = DateTime.UtcNow.ToString("r");
        var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString, m_configuration.Value.Key);

        m_client.DefaultRequestHeaders.Clear();
        m_client.DefaultRequestHeaders.Add("Accept", "application/json");
        m_client.DefaultRequestHeaders.Add("authorization", auth);
        m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
        m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");

        var requestUri = new Uri($"{m_configuration.Value.Host}/{resourceLink}");
        var httpRequest = new HttpRequestMessage { Method = method, RequestUri = requestUri };

        var httpResponse = await m_client.SendAsync(httpRequest);
        await m_reporter.ReportOutput($"Get Container with id: '{databaseId}' :", httpResponse);
    }


    public async Task DeleteContainer(string databaseId, string containerId)
    {
        var method = HttpMethod.Delete;

        var resourceType = ResourceType.colls;
        var resourceLink = $"dbs/{databaseId}/colls/{containerId}";
        var requestDateString = DateTime.UtcNow.ToString("r");
        var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString, m_configuration.Value.Key);

        m_client.DefaultRequestHeaders.Clear();
        m_client.DefaultRequestHeaders.Add("Accept", "application/json");
        m_client.DefaultRequestHeaders.Add("authorization", auth);
        m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
        m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");

        var requestUri = new Uri($"{m_configuration.Value.Host}/{resourceLink}");
        var httpRequest = new HttpRequestMessage { Method = method, RequestUri = requestUri };

        var httpResponse = await m_client.SendAsync(httpRequest);
        await m_reporter.ReportOutput("Delete Container", httpResponse);
    }

    public async Task GetContainerPartitionKeys(string databaseId, string containerId)
    {
        var method = HttpMethod.Get;

        var resourceType = ResourceType.pkranges;
        var resourceLink = $"dbs/{databaseId}/colls/{containerId}";
        var requestDateString = DateTime.UtcNow.ToString("r");
        var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString, m_configuration.Value.Key);

        m_client.DefaultRequestHeaders.Clear();
        m_client.DefaultRequestHeaders.Add("Accept", "application/json");
        m_client.DefaultRequestHeaders.Add("authorization", auth);
        m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
        m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");
        var requestUri = new Uri($"{m_configuration.Value.Host}/{resourceLink}/pkranges");
    
        var httpRequest = new HttpRequestMessage { Method = method,  RequestUri = requestUri };

        var httpResponse = await m_client.SendAsync(httpRequest);
        await m_reporter.ReportOutput($"Get Partition Key Ranges for collection '{containerId}':", httpResponse);
    }

}