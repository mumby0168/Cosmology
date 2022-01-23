using System.Reflection.Metadata;
using Cosmology.Extensions;
using Cosmology.Settings;
using Microsoft.Azure.Cosmos;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Cosmology.Commands.Database;

public class ListDatabases : AsyncCommand<DbCommonSettings>
{
    private readonly CosmosClient _cosmosClient;

    public ListDatabases(CosmosClient cosmosClient) =>
        _cosmosClient = cosmosClient;

    public override async Task<int> ExecuteAsync(CommandContext context, DbCommonSettings settings)
    {
        var iterator = _cosmosClient.GetDatabaseQueryIterator<DatabaseProperties>(Constants.SelectAllSql);

        await AnsiConsole.Status().StartAsync("Getting dbs", async ctx =>
        {
            var dbs = 1;
            
            await foreach (var database in _cosmosClient.GetAllDatabases())
            {
                AnsiConsole.WriteLine($"{dbs}. {database.Id}");
                ctx.Refresh();
                dbs++;
            }

            if (dbs is 1)
            {
                CosmologyOutput.Info("No databases");
            }
        });
        return 1;
    }
}