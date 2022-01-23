using System.Net;
using Cosmology.Settings;
using Microsoft.Azure.Cosmos;
using Spectre.Console.Cli;

namespace Cosmology.Commands.Database;

public class DeleteDatabase : AsyncCommand<DbCommonSettings>
{
    private readonly CosmosClient _cosmosClient;

    public DeleteDatabase(CosmosClient cosmosClient) => 
        _cosmosClient = cosmosClient;

    public override async Task<int> ExecuteAsync(CommandContext context, DbCommonSettings settings)
    {
        try
        {
            var database = _cosmosClient.GetDatabase(settings.DatabaseName);
            await database.DeleteAsync();
        }
        catch (CosmosException e) when (e.StatusCode is HttpStatusCode.NotFound)
        {
            CosmologyOutput.Error($"Database with the name {settings.DatabaseName} does not exist");
        }

        return 1;
    }
}