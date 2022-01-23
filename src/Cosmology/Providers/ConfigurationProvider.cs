using System.Reflection;
using System.Text.Json;
using Cosmology.Models;

namespace Cosmology.Providers;

public class ConfigurationProvider
{
    private readonly string _configDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

    public string GetCosmosConnectionString()
    {
        var text = File.ReadAllText(Path.Combine(_configDirectory, "appsettings.json"));
        var config = JsonSerializer.Deserialize<CosmologyConfig>(text);

        var connectionString = Environment.GetEnvironmentVariable(Constants.ConnectionStringEnvironmentVariable);

        if (connectionString is not null)
        {
            return connectionString;
        }

        if (config is null || config.CosmosConnectionString is null or "<connection-string-here>")
        {
            throw new Exception("Please provide a cosmos connection string");
        }

        return config.CosmosConnectionString;
    }
}