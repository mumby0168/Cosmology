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
    configurator.Settings.ApplicationName = "cosmology";
    configurator.AddBranch<DbCommonSettings>("dbs", dbConfigurator =>
    {
        dbConfigurator.SetDescription("Interacts with cosmos dbs associated with the account configured.");
        dbConfigurator.AddExample(new []{"delete", "-n my-db-to-delete"});
        dbConfigurator.AddExample(new []{"delete", "--database-name my-db-to-delete"});
        dbConfigurator.AddExample(new []{"list"});
        dbConfigurator.AddExample(new []{"prune"});
        dbConfigurator.AddExample(new []{"prune", "--skip-confirmation"});

        dbConfigurator.AddCommand<DeleteDatabase>("delete")
            .WithDescription("deletes a cosmos db database")
            .WithAlias("del")
            .WithExample(new[] {"delete","-n my-db-to-delete"})
            .WithExample(new []{"del", "--database-name my-db-to-delete"});
            
        dbConfigurator.AddCommand<ListDatabases>("list")
            .WithDescription("list all cosmos dbs on the current account")
            .WithAlias("l");
        
        dbConfigurator.AddCommand<PruneDatabases>("prune")
            .WithDescription("deletes ALL cosmos dbs from the current account");
    });
});

var result = await app.RunAsync(args);
return result;