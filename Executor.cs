using CosmosDbRestSamples.Operations;
using CosmosDbRestSamples.Types;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CosmosDbRestSamples;

public class Executor : IHostedService
{
    private readonly IOptions<Configuration> m_configuration;
    private readonly CollectionsOperations m_collectionsOperations;
    private readonly DatabaseOperations m_databaseOperations;
    private readonly DocumentsOperations m_documentsOperations;
    private readonly StoredProceduresOperations m_storedProceduresOperations;
    private readonly ILogger<Executor> m_logger;

    public Executor(
        IOptions<Configuration> configuration, 
        CollectionsOperations collectionsOperations,
        DatabaseOperations databaseOperations,
        DocumentsOperations documentsOperations,
        StoredProceduresOperations storedProceduresOperations,
        ILogger<Executor> logger)
    {
        m_configuration = configuration;
        m_collectionsOperations = collectionsOperations;
        m_databaseOperations = databaseOperations;
        m_documentsOperations = documentsOperations;
        m_storedProceduresOperations = storedProceduresOperations;
        m_logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var databaseId = "testdb";
        var containerId = "c1";
        var item1 = new ItemDto("id1", "pk1", "value1");
        var item11 = new ItemDto("id11", "pk1", "value-11");
        var item2 = new ItemDto("id2", "pk1", "value2");
        var item3 = new ItemDto("id3", "pk2", "value3");

        // await m_databaseOperations.CreateDatabase(databaseId, DatabaseThoughputMode.@fixed);

        // await m_databaseOperations.ListDatabases();
        // await m_databaseOperations.GetDatabase(databaseId);

        // await m_collectionsOperations.CreateContainer(databaseId, containerId, DatabaseThoughputMode.none);

        // await m_collectionsOperations.GetContainer(databaseId, containerId);
        // await m_collectionsOperations.GetContainerPartitionKeys(databaseId, containerId);

        // await m_storedProceduresOperations.CreateStoredProcedure(databaseId, containerId, "sproc1");
        // await m_storedProceduresOperations.DeleteStoredProcedure(databaseId, containerId, "sproc1");

        await m_documentsOperations.CreateDocument(databaseId, containerId, item1);
        // await m_documentsOperations.CreateDocument(databaseId, containerId, item2);
        // await m_documentsOperations.CreateDocument(databaseId, containerId, item3);

        // await m_documentsOperations.PatchDocument(databaseId, containerId, id: item1.id, partitionKey: item1.pk);
        // await m_documentsOperations.ReplaceDocument(databaseId, containerId, id: item1.id, newItem: item11); //cannot change partitionKey in a replace operation, but can update id

        // await m_documentsOperations.ListDocuments(databaseId, containerId, partitionKey: item1.pk);
        // await m_documentsOperations.GetDocument(databaseId, containerId, id: item2.id, partitionKey: item2.pk);

        // await m_documentsOperations.QueryDocuments(databaseId, containerId, partitionKey: item1.pk);
        // await m_documentsOperations.QueryDocumentsCrossPartition(databaseId, containerId);

        // await m_documentsOperations.DeleteDocument(databaseId, containerId, id: item11.id, partitionKey: item11.pk);
        // await m_documentsOperations.DeleteDocument(databaseId, containerId, id: item2.id, partitionKey: item2.pk);
        // await m_documentsOperations.DeleteDocument(databaseId, containerId, id: item3.id, partitionKey: item3.pk);

        // await m_collectionsOperations.DeleteContainer(databaseId, containerId);

        // await m_databaseOperations.DeleteDatabase(databaseId);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
