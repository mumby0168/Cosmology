using Spectre.Console.Cli;

namespace Cosmology.Settings;

public class SetConfigSettings : CommandSettings
{
    [CommandArgument(0, "<key>")]
    public string Key { get; set; } = default!;
    
    [CommandArgument(1, "<value>")]
    public string Value { get; set; } = default!;
}