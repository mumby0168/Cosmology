using Cosmology.Extensions;
using Cosmology.Settings;
using Microsoft.Azure.Cosmos;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Cosmology.Commands.Database;

public class PruneDatabases : AsyncCommand<DbPruneSettings>
{
    private readonly CosmosClient _cosmosClient;

    public PruneDatabases(CosmosClient cosmosClient) =>
        _cosmosClient = cosmosClient;

    public override async Task<int> ExecuteAsync(CommandContext context, DbPruneSettings settings)
    {
        if (settings.SkipConfirmation)
        {
            await PruneAllDatabases();
            return 1;
        }

        if (AnsiConsole.Confirm("Are you sure you want to delete ALL databases?"))
        {
            await PruneAllDatabases();
            return 1;
        }

        return 1;
    }

    private async Task PruneAllDatabases()
    {
        await AnsiConsole.Status().StartAsync("Pruning databases", async _ =>
        {
            int dbs = 0;
            await foreach (var databaseProperties in _cosmosClient.GetAllDatabases())
            {
                var db = _cosmosClient.GetDatabase(databaseProperties.Id);
                CosmologyOutput.Info($"Deleting db {db.Id}");
                await db.DeleteAsync();
                CosmologyOutput.Success($"Deleted db {db.Id}");
                dbs++;
            }

            if (dbs is 0)
            {
                AnsiConsole.WriteLine("No databases to delete");
            }
        });
    }
}