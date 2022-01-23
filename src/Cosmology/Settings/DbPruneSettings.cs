using Spectre.Console.Cli;

namespace Cosmology.Settings;

public class DbPruneSettings : DbCommonSettings
{
    [CommandOption("--skip-confirmation")] 
    public bool SkipConfirmation { get; set; } = false;
}