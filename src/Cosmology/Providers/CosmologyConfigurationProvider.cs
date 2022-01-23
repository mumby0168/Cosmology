using System.Reflection;
using System.Text.Json;
using Cosmology.Models;

namespace Cosmology.Providers;

public class CosmologyConfigurationProvider
{
    private readonly string _configDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

    public string? GetCosmosConnectionString()
    {
        var text = File.ReadAllText(Path.Combine(_configDirectory, "appsettings.json"));
        var config = JsonSerializer.Deserialize<CosmologyConfig>(text);

        var connectionString = Environment.GetEnvironmentVariable(Constants.ConnectionStringEnvironmentVariable);

        if (connectionString is not null)
        {
            return connectionString;
        }

        if (config is null || config.CosmosConnectionString is null or "NOT_SET")
        {
            return null;
        }

        return config.CosmosConnectionString;
    }

    public CosmologyConfig GetConfig()
    {
        var text = File.ReadAllText(Path.Combine(_configDirectory, "appsettings.json"));
        var config = JsonSerializer.Deserialize<CosmologyConfig>(text);

        return config!;
    }

    public void SaveConfig(CosmologyConfig config)
    {
        var text = JsonSerializer.Serialize(config, new JsonSerializerOptions {WriteIndented = true});
        File.WriteAllText(Path.Combine(_configDirectory, "appsettings.json"), text);
    }
}