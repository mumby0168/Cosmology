using Cosmology;
using Cosmology.Commands.Database;
using Cosmology.Extensions;
using Cosmology.Providers;
using Cosmology.Settings;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

var services = new ServiceCollection();

services.AddSingleton(sp =>
{
    var configProvider = sp.GetRequiredService<ConfigurationProvider>();
    var connectionString = configProvider.GetCosmosConnectionString();

    if (connectionString is not null)
    {
        return new CosmosClient(connectionString);
    }

    CosmologyOutput.Error(
        $"Please set the {Constants.ConnectionStringEnvironmentVariable} env var to a valid cosmos connection string.");
    
    throw new Exception();
});

services.AddSingleton<ConfigurationProvider>();
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