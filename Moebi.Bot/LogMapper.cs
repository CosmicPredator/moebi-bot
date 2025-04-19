using Discord;
using Serilog;
using Serilog.Events;

namespace Moebi.Bot;

public static class LogMapper
{
    public static async Task SerilogMapper(LogMessage logMessage)
    {
        var logLevel = logMessage.Severity switch
        {
            LogSeverity.Critical => LogEventLevel.Fatal,
            LogSeverity.Error => LogEventLevel.Error,
            LogSeverity.Warning => LogEventLevel.Warning,
            LogSeverity.Info => LogEventLevel.Information,
            LogSeverity.Verbose => LogEventLevel.Verbose,
            LogSeverity.Debug => LogEventLevel.Debug,
            _ => LogEventLevel.Debug
        };
        Log.ForContext("SourceContext", logMessage.Source)
            .Write(logLevel, logMessage.Exception, "{message:l}", logMessage.Message);
        await Task.CompletedTask;
    }
}