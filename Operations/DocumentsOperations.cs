using System.Net.Http.Headers;
using System.Text.Json;
using CosmosDbRestSamples.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CosmosDbRestSamples.Operations;

public class DocumentsOperations
{
    private readonly IOptions<Configuration> m_configuration;
    private readonly Security m_security;
    private readonly HttpClient m_client;
    private readonly OutputReporter m_reporter;
    private readonly ILogger<CollectionsOperations> m_logger;

    public DocumentsOperations(
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

    public async Task CreateDocument(string databaseId, string containerId, ItemDto item)
    {
        var method = HttpMethod.Post;

        var resourceType = ResourceType.docs;
        var resourceLink = $"dbs/{databaseId}/colls/{containerId}";
        var requestDateString = DateTime.UtcNow.ToString("r");
        var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString,  m_configuration.Value.Key);

        m_client.DefaultRequestHeaders.Clear();
        m_client.DefaultRequestHeaders.Add("Accept", "application/json");
        m_client.DefaultRequestHeaders.Add("authorization", auth);
        m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");
        m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
        m_client.DefaultRequestHeaders.Add("x-ms-documentdb-is-upsert", "True");
        m_client.DefaultRequestHeaders.Add("x-ms-documentdb-partitionkey", $"[\"{item.pk}\"]");

        var requestUri = new Uri($"{ m_configuration.Value.Host}/{resourceLink}/docs");
        var requestContent = new StringContent(JsonSerializer.Serialize(item), System.Text.Encoding.UTF8, "application/json");

        var httpRequest = new HttpRequestMessage { Method = method, Content = requestContent, RequestUri = requestUri };

        var httpResponse = await m_client.SendAsync(httpRequest);
        await m_reporter.ReportOutput("Create Document", httpResponse);
    }

    public async Task ListDocuments(string databaseId, string containerId, string partitionKey)
    {
        var method = HttpMethod.Get;
        var resourceType = ResourceType.docs;
        var resourceLink = $"dbs/{databaseId}/colls/{containerId}";
        var requestDateString = DateTime.UtcNow.ToString("r");
        var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString,  m_configuration.Value.Key);

        m_client.DefaultRequestHeaders.Clear();
        m_client.DefaultRequestHeaders.Add("Accept", "application/json");
        m_client.DefaultRequestHeaders.Add("authorization", auth);
        m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
        m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");
        m_client.DefaultRequestHeaders.Add("x-ms-documentdb-partitionkey", $"[\"{partitionKey}\"]");

        var requestUri = new Uri($"{ m_configuration.Value.Host}/{resourceLink}/docs");
        var httpRequest = new HttpRequestMessage { Method = method, RequestUri = requestUri };

        var httpResponse = await m_client.SendAsync(httpRequest);
        await m_reporter.ReportOutput($"List Documents for partitionKey {partitionKey}", httpResponse);
    }

    public async Task GetDocument(string databaseId, string containerId, string id, string partitionKey)
    {
        var method = HttpMethod.Get;
        var resourceType = ResourceType.docs;
        var resourceLink = $"dbs/{databaseId}/colls/{containerId}/docs/{id}";
        var requestDateString = DateTime.UtcNow.ToString("r");
        var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString,  m_configuration.Value.Key);

        m_client.DefaultRequestHeaders.Clear();
        m_client.DefaultRequestHeaders.Add("Accept", "application/json");
        m_client.DefaultRequestHeaders.Add("authorization", auth);
        m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
        m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");
        m_client.DefaultRequestHeaders.Add("x-ms-documentdb-partitionkey", $"[\"{partitionKey}\"]");

        var requestUri = new Uri($"{ m_configuration.Value.Host}/{resourceLink}");
        var httpRequest = new HttpRequestMessage { Method = method, RequestUri = requestUri };

        var httpResponse = await m_client.SendAsync(httpRequest);
        await m_reporter.ReportOutput($"Get Document by id: '{id}'", httpResponse);
    }

    public async Task ReplaceDocument(string databaseId, string containerId, string id, ItemDto newItem)
    {
        var method = HttpMethod.Put;

        var resourceType = ResourceType.docs;
        var resourceLink = $"dbs/{databaseId}/colls/{containerId}/docs/{id}";
        var requestDateString = DateTime.UtcNow.ToString("r");
        var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString,  m_configuration.Value.Key);

        m_client.DefaultRequestHeaders.Clear();
        m_client.DefaultRequestHeaders.Add("Accept", "application/json");
        m_client.DefaultRequestHeaders.Add("authorization", auth);
        m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
        m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");
        m_client.DefaultRequestHeaders.Add("x-ms-documentdb-partitionkey", $"[\"{newItem.pk}\"]");

        var requestUri = new Uri($"{ m_configuration.Value.Host}/{resourceLink}");
        var requestContent = new StringContent(JsonSerializer.Serialize(newItem), System.Text.Encoding.UTF8, "application/json");

        var httpRequest = new HttpRequestMessage { Method = method, Content = requestContent, RequestUri = requestUri };

        var httpResponse = await m_client.SendAsync(httpRequest);
        await m_reporter.ReportOutput($"Replace Document with id '{id}'", httpResponse);

    }

    public async Task PatchDocument(string databaseId, string containerId, string id, string partitionKey)
    {
        var method = HttpMethod.Patch;

        var resourceType = ResourceType.docs;
        var resourceLink = $"dbs/{databaseId}/colls/{containerId}/docs/{id}";
        var requestDateString = DateTime.UtcNow.ToString("r");
        var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString,  m_configuration.Value.Key);

        m_client.DefaultRequestHeaders.Clear();
        m_client.DefaultRequestHeaders.Add("Accept", "application/json");
        m_client.DefaultRequestHeaders.Add("authorization", auth);
        m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
        m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");
        m_client.DefaultRequestHeaders.Add("x-ms-documentdb-partitionkey", $"[\"{partitionKey}\"]");

        var requestUri = new Uri($"{ m_configuration.Value.Host}/{resourceLink}");
        var requestBody = @"
    {
    ""operations"": [
        {
        ""op"": ""set"",
        ""path"": ""/someProperty"",
        ""value"": ""value-patched""
        }
    ]
    }  ";

        var requestContent = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/json");

        var httpRequest = new HttpRequestMessage { Method = method, Content = requestContent, RequestUri = requestUri };

        var httpResponse = await m_client.SendAsync(httpRequest);
        await m_reporter.ReportOutput($"Patch Document with id '{id}'", httpResponse);
    }


    public async Task DeleteDocument(string databaseId, string containerId, string id, string partitionKey)
    {
        var method = HttpMethod.Delete;
        var resourceType = ResourceType.docs;
        var resourceLink = $"dbs/{databaseId}/colls/{containerId}/docs/{id}";
        var requestDateString = DateTime.UtcNow.ToString("r");
        var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString,  m_configuration.Value.Key);

        m_client.DefaultRequestHeaders.Clear();
        m_client.DefaultRequestHeaders.Add("Accept", "application/json");
        m_client.DefaultRequestHeaders.Add("authorization", auth);
        m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
        m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");
        m_client.DefaultRequestHeaders.Add("x-ms-documentdb-partitionkey", $"[\"{partitionKey}\"]");

        var requestUri = new Uri($"{ m_configuration.Value.Host}/{resourceLink}");
        var httpRequest = new HttpRequestMessage { Method = method, RequestUri = requestUri };

        var httpResponse = await m_client.SendAsync(httpRequest);

        await m_reporter.ReportOutput($"Deleted item with id '{id}':", httpResponse);
    }


    public async Task QueryDocuments(string databaseId, string containerId, string partitionKey)
    {
        var method = HttpMethod.Post;
        var resourceType = ResourceType.docs;
        var resourceLink = $"dbs/{databaseId}/colls/{containerId}";
        var requestDateString = DateTime.UtcNow.ToString("r");
        var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString,  m_configuration.Value.Key);

        m_client.DefaultRequestHeaders.Clear();
        m_client.DefaultRequestHeaders.Add("Accept", "application/json");
        m_client.DefaultRequestHeaders.Add("authorization", auth);
        m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
        m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");
        m_client.DefaultRequestHeaders.Add("x-ms-documentdb-isquery", "True");

        var requestUri = new Uri($"{ m_configuration.Value.Host}/{resourceLink}/docs");
        var requestBody = @$"
    {{  
    ""query"": ""SELECT * FROM c WHERE c.pk = @pk"",  
    ""parameters"": [
        {{  
        ""name"": ""@pk"",
        ""value"": ""{partitionKey}""  
        }}
    ]  
    }}";
        StringContent requestContent = new(requestBody, System.Text.Encoding.UTF8, "application/query+json");
        if (requestContent.Headers.ContentType is null)
        {
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        //NOTE -> this is important. CosmosDB expects a specific Content-Type with no CharSet on a query request.
        requestContent.Headers.ContentType.CharSet = "";

        var httpRequest = new HttpRequestMessage { Method = method, Content = requestContent, RequestUri = requestUri };

        var httpResponse = await m_client.SendAsync(httpRequest);

        await m_reporter.ReportOutput("Query: ", httpResponse);
    }

    public async Task QueryDocumentsCrossPartition(string databaseId, string containerId)
    {
        var method = HttpMethod.Post;
        var resourceType = ResourceType.docs;
        var resourceLink = $"dbs/{databaseId}/colls/{containerId}";
        var requestDateString = DateTime.UtcNow.ToString("r");
        var auth = m_security.GenerateMasterKeyAuthorizationSignature(method, resourceType, resourceLink, requestDateString,  m_configuration.Value.Key);

        m_client.DefaultRequestHeaders.Clear();
        m_client.DefaultRequestHeaders.Add("Accept", "application/json");
        m_client.DefaultRequestHeaders.Add("authorization", auth);
        m_client.DefaultRequestHeaders.Add("x-ms-date", requestDateString);
        m_client.DefaultRequestHeaders.Add("x-ms-version", "2018-12-31");
        m_client.DefaultRequestHeaders.Add("x-ms-max-item-count", "2");
        m_client.DefaultRequestHeaders.Add("x-ms-documentdb-query-enablecrosspartition", "True");
        m_client.DefaultRequestHeaders.Add("x-ms-documentdb-isquery", "True");

        var requestUri = new Uri($"{ m_configuration.Value.Host}/{resourceLink}/docs");
        var requestBody = @$"
    {{  
    ""query"": ""SELECT * FROM c"",  
    ""parameters"": []  
    }}";

        var requestContent = new StringContent(requestBody, System.Text.Encoding.UTF8, "application/query+json");
        if (requestContent.Headers.ContentType is null)
        {
            requestContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        //NOTE -> this is important. CosmosDB expects a specific Content-Type with no CharSet on a query request.
        requestContent.Headers.ContentType.CharSet = "";
        var httpRequest = new HttpRequestMessage { Method = method, Content = requestContent, RequestUri = requestUri };

        var httpResponse = await m_client.SendAsync(httpRequest);
        //var continuation = httpResponse.Headers.GetValues("x-ms-continuation");

        await m_reporter.ReportOutput("Query: ", httpResponse);
    }
}