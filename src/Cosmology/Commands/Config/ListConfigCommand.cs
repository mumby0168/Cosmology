using System.Text.Json;
using Cosmology.Models;
using Cosmology.Providers;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Cosmology.Commands.Config;

public class ListConfigCommand : Command
{
    private readonly CosmologyConfigurationProvider _cosmologyConfigurationProvider;

    public ListConfigCommand(CosmologyConfigurationProvider cosmologyConfigurationProvider) => 
        _cosmologyConfigurationProvider = cosmologyConfigurationProvider;

    public override int Execute(CommandContext context)
    {
        CosmologyConfig config = _cosmologyConfigurationProvider.GetConfig();

        var rule = new Rule("Cosmos Config")
        {
            Alignment = Justify.Left
        };
        AnsiConsole.Write(rule);
        
        CosmologyOutput.Info(JsonSerializer.Serialize(config, new JsonSerializerOptions()
        {
            WriteIndented = true,
        }));

        return 1;
    }
}