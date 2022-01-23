using Cosmology.Models;
using Cosmology.Providers;
using Cosmology.Settings;
using Spectre.Console.Cli;

namespace Cosmology.Commands.Config;

public class SetConfigPropertyCommand : AsyncCommand<SetConfigSettings>
{
    private readonly CosmologyConfigurationProvider _configurationProvider;

    public SetConfigPropertyCommand(CosmologyConfigurationProvider configurationProvider) => 
        _configurationProvider = configurationProvider;

    public override async Task<int> ExecuteAsync(CommandContext context, SetConfigSettings settings)
    {
        await Task.CompletedTask;
        
        if (!IsConfigProperty(settings.Key))
        {
            CosmologyOutput.Error($"{settings.Key} is not a valid config value");
            return 1;
        }
        
        var config = _configurationProvider.GetConfig();
        SetConfigProperty(config, settings.Key, settings.Value);
        _configurationProvider.SaveConfig(config);
        
        CosmologyOutput.Success($"Successfully set config value {settings.Key} = {settings.Value}");
        
        return 1;
    }

    private static void SetConfigProperty(CosmologyConfig config, string key, string value)
    {
        var type = typeof(CosmologyConfig);
        
        foreach (var property in type.GetProperties())
        {
            if (property.Name == key)
            {
                property.SetValue(config, value);
            }
        }
    }

    private static bool IsConfigProperty(string key)
    {
        var type = typeof(CosmologyConfig);

        foreach (var property in type.GetProperties())
        {
            if (property.Name == key)
            {
                return true;
            }
        }

        return false;
    }
}