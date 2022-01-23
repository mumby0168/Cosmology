using Microsoft.Azure.Cosmos;

namespace Cosmology.Extensions;

public static class CosmosClientExtensions
{
    public static async IAsyncEnumerable<DatabaseProperties> GetAllDatabases(this CosmosClient cosmosClient)
    {
        var iterator = cosmosClient.GetDatabaseQueryIterator<DatabaseProperties>(Constants.SelectAllSql);

        while (iterator.HasMoreResults)
        {
            var next = await iterator.ReadNextAsync();
            foreach (var properties in next)
            {
                yield return properties;
            }
        }
    }
}