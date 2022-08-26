using CosmosDbRestSamples.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CosmosDbRestSamples.Operations;

public class StoredProceduresOperations
{
    private readonly IOptions<Configuration> m_configuration;
    private readonly Security m_security;
    private readonly HttpClient m_client;
    private readonly OutputReporter m_reporter;
    private readonly ILogger<CollectionsOperations> m_logger;

    public StoredProceduresOperations(
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

    public async Task CreateStoredProcedure(string databaseId, string containerId, string storedProcedureName)
    {
        var method = HttpMethod.Post;

        var resourceType = ResourceType.sprocs;
        var resourceLink = $"dbs/{databaseId}/colls/{containerId}";
        var requestDateString = DateTime.UtcNow.ToString("r");
        var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString,  m_configuration.Value.Key);

        m_client.DefaultRequestHeaders.Clear();
        m_client.DefaultRequestHeaders.Add("Accept", "application/json");
        m_client.DefaultRequestHeaders.Add("authorization", auth);
        m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
        m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");

        var requestUri = new Uri($"{ m_configuration.Value.Host}/{resourceLink}/sprocs");
        var requestBody = $@"{{
        ""body"": ""function () {{ var context = getContext(); var response = context.getResponse(); response.setBody(\""Hello, World\"");}}"",
        ""id"":""{storedProcedureName}""
    }}";

        var requestContent = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");
        var httpRequest = new HttpRequestMessage { Method = method, Content = requestContent, RequestUri = requestUri };

        var httpResponse = await m_client.SendAsync(httpRequest);
        await m_reporter.ReportOutput($"Create Stored procedure '{storedProcedureName}' on container '{containerId}' :", httpResponse);
    }

    public async Task DeleteStoredProcedure(string databaseId, string containerId, string storedProcedureName)
    {
        var method = HttpMethod.Delete;

        var resourceType = ResourceType.sprocs;
        var resourceLink = $"dbs/{databaseId}/colls/{containerId}/sprocs/{storedProcedureName}";
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
        await m_reporter.ReportOutput($"Delete Stored Procedure '{storedProcedureName}", httpResponse);
    }
}
