using CosmosDbRestSamples.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CosmosDbRestSamples.Operations;

public class DatabaseOperations
{
    private readonly IOptions<Configuration> m_configuration;
    private readonly Security m_security;
    private readonly HttpClient m_client;
    private readonly OutputReporter m_reporter;
    private readonly ILogger<CollectionsOperations> m_logger;

    public DatabaseOperations(
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

    public async Task CreateDatabase(string databaseId, DatabaseThoughputMode mode)
    {
        try
        {
            var method = HttpMethod.Post;

            var resourceType = ResourceType.dbs;
            var resourceLink = $"";
            var requestDateString = DateTime.UtcNow.ToString("r");
            var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString,  m_configuration.Value.Key);

            m_client.DefaultRequestHeaders.Clear();
            m_client.DefaultRequestHeaders.Add("Accept", "application/json");
            m_client.DefaultRequestHeaders.Add("authorization", auth);
            m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
            m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");

            if (mode == DatabaseThoughputMode.@fixed)
                m_client.DefaultRequestHeaders.Add("x-ms-offer-throughput", "400");
            if (mode == DatabaseThoughputMode.autopilot)
                m_client.DefaultRequestHeaders.Add("x-ms-cosmos-offer-autopilot-settings", "{\"maxThroughput\": 4000}");

            var requestUri = new Uri($"{ m_configuration.Value.Host}/dbs");
            var requestBody = $"{{\"id\":\"{databaseId}\"}}";
            var requestContent = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");

            var httpRequest = new HttpRequestMessage { Method = method, Content = requestContent, RequestUri = requestUri };

            var httpResponse = await m_client.SendAsync(httpRequest);
            await m_reporter.ReportOutput($"Create Database with thoughput mode {mode}:", httpResponse);
        }
        catch (Exception ex)
        {
            m_logger.LogError(ex, "An error occurred.");
        }
    }

    public async Task ListDatabases()
    {
        var method = HttpMethod.Get;

        var resourceType = ResourceType.dbs;
        var resourceLink = $"";
        var requestDateString = DateTime.UtcNow.ToString("r");
        var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString,  m_configuration.Value.Key);

        m_client.DefaultRequestHeaders.Clear();
        m_client.DefaultRequestHeaders.Add("Accept", "application/json");
        m_client.DefaultRequestHeaders.Add("authorization", auth);
        m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
        m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");

        var requestUri = new Uri($"{ m_configuration.Value.Host}/dbs");
        var httpRequest = new HttpRequestMessage { Method = method, RequestUri = requestUri };

        var httpResponse = await m_client.SendAsync(httpRequest);
        await m_reporter.ReportOutput($"List Databases:", httpResponse);
    }

    public async Task GetDatabase(string databaseId)
    {
        var method = HttpMethod.Get;

        var resourceType = ResourceType.dbs;
        var resourceLink = $"dbs/{databaseId}";
        var requestDateString = DateTime.UtcNow.ToString("r");
        var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString,  m_configuration.Value.Key);

        m_client.DefaultRequestHeaders.Clear();
        m_client.DefaultRequestHeaders.Add("Accept", "application/json");
        m_client.DefaultRequestHeaders.Add("authorization", auth);
        m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
        m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");

        var requestUri = new Uri($"{ m_configuration.Value.Host}/{resourceLink}");
        var httpRequest = new HttpRequestMessage { Method = method, RequestUri = requestUri };

        var httpResponse = await m_client.SendAsync(httpRequest);
        await m_reporter.ReportOutput($"Get Database with id: '{databaseId}' :", httpResponse);
    }

    public async Task DeleteDatabase(string databaseId)
    {
        var method = HttpMethod.Delete;

        var resourceType = ResourceType.dbs;
        var resourceLink = $"dbs/{databaseId}";
        var requestDateString = DateTime.UtcNow.ToString("r");
        var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString,  m_configuration.Value.Key);

        m_client.DefaultRequestHeaders.Clear();
        m_client.DefaultRequestHeaders.Add("Accept", "application/json");
        m_client.DefaultRequestHeaders.Add("authorization", auth);
        m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
        m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");

        var requestUri = new Uri($"{ m_configuration.Value.Host}/{resourceLink}");
        var httpRequest = new HttpRequestMessage { Method = method, RequestUri = requestUri };

        var httpResponse = await m_client.SendAsync(httpRequest);
        await m_reporter.ReportOutput("Delete Database", httpResponse);
    }

}