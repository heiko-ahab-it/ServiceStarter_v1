using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog.Events;
using Serilog;
using Serilog.Enrichers.CallerInfo;
using Microsoft.Extensions.Options;
using ConfigLoader_v2;

namespace ServiceStarter_v1.Validation_Config
{
    public static class LoggingExtensions
    {
        
        public static void ConfigureCustomSerilog(this IHostApplicationBuilder builder,string logDir)
        {
            var template = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] [{SourceFile}:{Method}:{LineNumber}] {Message:lj}{NewLine}{Exception}";

            string _logDir = logDir.TrimEnd('\\', '/');

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .Enrich.WithCallerInfo(
                    includeFileInfo: true,
                    assemblyPrefix: "ServiceStarter_v1",
                    filePathDepth: 1,
                    prefix: "",
                    excludedPrefixes: new List<string> { "SomeOtherProject." })
                // Konsole
                .WriteTo.Console(outputTemplate: template)
                // Debug-Datei
                /*.WriteTo.File("D:\\LOG\\debug_.log",
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Debug,
                    outputTemplate: template)*/
                .WriteTo.File(Path.Combine(_logDir,"allLogLevel_.log"),//_logDir ,//"D:\\LOG\\allLogLevel_.log",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: template)

               /* // Info-Datei
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(evt =>
                       // evt.Level >= LogEventLevel.Information &&
                        evt.Level < LogEventLevel.Error)
                    .WriteTo.File("D:\\LOG\\info_.log",
                        restrictedToMinimumLevel: LogEventLevel.Information,
                        rollingInterval: RollingInterval.Day, outputTemplate: template))*/
                // Error-Datei
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(evt => evt.Level >= LogEventLevel.Error)
                    .WriteTo.File(Path.Combine(_logDir, "ERROR_.log"),//"D:\\LOG\\error_.log",
                        restrictedToMinimumLevel: LogEventLevel.Error,
                        rollingInterval: RollingInterval.Day, outputTemplate: template))
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();
        }
    }
}

