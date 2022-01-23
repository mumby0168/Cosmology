using Cosmology;
using Cosmology.Commands.Config;
using Cosmology.Commands.Database;
using Cosmology.Exceptions;
using Cosmology.Extensions;
using Cosmology.Providers;
using Cosmology.Settings;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

var services = new ServiceCollection();

services.AddSingleton(sp =>
{
    var configProvider = sp.GetRequiredService<CosmologyConfigurationProvider>();
    var connectionString = configProvider.GetCosmosConnectionString();

    if (connectionString is not null)
    {
        return new CosmosClient(connectionString);
    }

    throw new CosmosDbConnectionStringMissingException();
});

services.AddSingleton<CosmologyConfigurationProvider>();
services.AddSingleton(new DbCommonSettings());
services.AddSingleton(new DbPruneSettings());
services.AddSingleton(new EmptyCommandSettings());
services.AddSingleton(new SetConfigSettings());

var app = new CommandApp(services.BuildTypeRegistrar());

app.Configure(configurator =>
{
    configurator.Settings.ExceptionHandler = exception =>
    {
        if (exception.InnerException is not null &&
            exception.InnerException.IsOfType<CosmosDbConnectionStringMissingException>())
        {
            CosmologyOutput.Error(exception.InnerException.Message);
        }
        else
        {
            CosmologyOutput.Error("Oops, Something went wrong!");
            CosmologyOutput.Exception(exception);
        }

        return 1;
    };

    configurator.Settings.ApplicationName = "cosmology";
    configurator.AddBranch<DbCommonSettings>("dbs", dbCfg =>
    {
        dbCfg.SetDescription("Interacts with cosmos dbs associated with the account configured.");
        dbCfg.AddExample(new[] {"delete", "-n my-db-to-delete"});
        dbCfg.AddExample(new[] {"delete", "--database-name my-db-to-delete"});
        dbCfg.AddExample(new[] {"list"});
        dbCfg.AddExample(new[] {"prune"});
        dbCfg.AddExample(new[] {"prune", "--skip-confirmation"});

        dbCfg.AddCommand<DeleteDatabase>("delete")
            .WithDescription("deletes a cosmos db database")
            .WithAlias("del")
            .WithExample(new[] {"delete", "-n my-db-to-delete"})
            .WithExample(new[] {"del", "--database-name my-db-to-delete"});

        dbCfg.AddCommand<ListDatabases>("list")
            .WithDescription("list all cosmos dbs on the current account")
            .WithAlias("l");

        dbCfg.AddCommand<PruneDatabases>("prune")
            .WithDescription("deletes ALL cosmos dbs from the current account");
    });
    
    configurator.AddBranch("config", settingsCfg =>
    {
        settingsCfg.SetDescription("Manage settings for the cosmology tools");
        settingsCfg.AddCommand<ListConfigCommand>("list");
        settingsCfg.AddCommand<SetConfigPropertyCommand>("set");
    });
});

var result = await app.RunAsync(args);
return result;