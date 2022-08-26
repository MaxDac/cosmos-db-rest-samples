using CosmosDbRestSamples;
using CosmosDbRestSamples.Operations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

try
{
    var builder = Host.CreateDefaultBuilder();

    builder.ConfigureServices((host, services) =>
    {
        services.AddSingleton<OutputReporter>();
        services.AddSingleton<Security>();
        services.AddHttpClient<DatabaseOperations>();
        services.AddHttpClient<CollectionsOperations>();
        services.AddHttpClient<DocumentsOperations>();
        services.AddHttpClient<StoredProceduresOperations>();
        services.Configure<Configuration>(host.Configuration.GetSection("Cosmos"));

        services.AddHostedService<Executor>();
    });

    await builder.Build().RunAsync();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}
// var  m_configuration.Value.Key = Environment.GetEnvironmentVariable("Cosmos:Key");
// var accountName = Environment.GetEnvironmentVariable("Cosmos:AccountName");

// if (string.IsNullOrEmpty(cosmosKey) || string.IsNullOrEmpty(accountName))
// {
//     Console.WriteLine("Missing one or more configuration values. Please make sure to set them in the `environmenVariables` section");
//     return;
// }

// var  m_configuration.Value.Host = $"https://{accountName}.documents.azure.com";
// var m_client = new m_client();

// var databaseId = "testdb";
// var containerId = "c1";
// var item1 = new ItemDto("id1", "pk1", "value1");
// var item11 = new ItemDto("id11", "pk1", "value-11");
// var item2 = new ItemDto("id2", "pk1", "value2");
// var item3 = new ItemDto("id3", "pk2", "value3");


// await CreateDatabase(databaseId, DatabaseThoughputMode.@fixed);

// await ListDatabases();
// await GetDatabase(databaseId);

// await CreateContainer(databaseId, containerId, DatabaseThoughputMode.none);

// await GetContainer(databaseId, containerId);
// await GetContainerPartitionKeys(databaseId, containerId);

// await CreateStoredProcedure(databaseId, containerId, "sproc1");
// await DeleteStoredProcedure(databaseId, containerId, "sproc1");

// await CreateDocument(item1);
// await CreateDocument(item2);
// await CreateDocument(item3);

// await PatchDocument(id: item1.id, partitionKey: item1.pk);
// await ReplaceDocument(id: item1.id, newItem: item11); //cannot change partitionKey in a replace operation, but can update id


// await ListDocuments(partitionKey: item1.pk);
// await GetDocument(id: item2.id, partitionKey: item2.pk);

// await QueryDocuments(partitionKey: item1.pk);
// await QueryDocumentsCrossPartition();

// await DeleteDocument(id: item11.id, partitionKey: item11.pk);
// await DeleteDocument(id: item2.id, partitionKey: item2.pk);
// await DeleteDocument(id: item3.id, partitionKey: item3.pk);


// await DeleteContainer(databaseId, containerId);

// await DeleteDatabase(databaseId);
