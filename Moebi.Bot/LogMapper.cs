using Discord;
using Serilog;
using Serilog.Events;

namespace Moebi.Bot;

/// <summary>
/// Provides a method to map Discord.NET log messages to Serilog logs.
/// </summary>
public static class LogMapper
{
    /// <summary>
    /// Maps a <see cref="LogMessage"/> from Discord.NET to Serilog, preserving log level, message, and exception details.
    /// </summary>
    /// <param name="logMessage">The log message received from Discord.NET.</param>
    public static async Task SerilogMapper(LogMessage logMessage)
    {
        var logLevel = logMessage.Severity switch
        {
            LogSeverity.Critical => LogEventLevel.Fatal,
            LogSeverity.Error => LogEventLevel.Error,
            LogSeverity.Warning => LogEventLevel.Warning,
            LogSeverity.Info => LogEventLevel.Information,
            LogSeverity.Verbose => LogEventLevel.Verbose,
            _ => LogEventLevel.Debug
        };
        Log.ForContext("SourceContext", logMessage.Source)
            .Write(logLevel, logMessage.Exception, "{message:l}", logMessage.Message);
        await Task.CompletedTask;
    }
}