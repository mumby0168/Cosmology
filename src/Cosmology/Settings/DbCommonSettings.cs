using Spectre.Console.Cli;

namespace Cosmology.Settings;

public class DbCommonSettings : CommandSettings
{
    [CommandOption("-n|--database-name <databaseName>")]
    public string DatabaseName { get; set; } = default!;
}