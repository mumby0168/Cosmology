using Cosmology.Commands.Database;
using Cosmology.Extensions;
using Cosmology.Providers;
using Cosmology.Settings;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var configProvider = new ConfigurationProvider();
var client = new CosmosClient(configProvider.GetCosmosConnectionString());
var services = new ServiceCollection();
services.AddSingleton(client);
services.AddSingleton(configProvider);
services.AddSingleton(new DbCommonSettings());
services.AddSingleton(new DbPruneSettings());

var app = new CommandApp(services.BuildTypeRegistrar());

app.Configure(configurator =>
{
    configurator.AddBranch<DbCommonSettings>("db", dbConfigurator =>
    {
        dbConfigurator.AddCommand<DeleteDatabase>("delete");
        dbConfigurator.AddCommand<ListDatabases>("list");
        dbConfigurator.AddCommand<PruneDatabases>("prune");
    });
});

var result = await app.RunAsync(args);
return result;