using Spectre.Console;

namespace Cosmology;

public static class CosmologyOutput
{
    public static void Error(string message) =>
        AnsiConsole.MarkupLine($"[red]{message}[/]");

    public static void Success(string message) =>
        AnsiConsole.MarkupLine($"[green]{message}[/]");

    public static void Info(string message) =>
        AnsiConsole.MarkupLine(message);

    public static void Exception(Exception e)
    {
        AnsiConsole.WriteException(e, new ExceptionSettings()
        {
            Format = ExceptionFormats.ShortenEverything,
            Style = new ExceptionStyle()
        });
    }
}