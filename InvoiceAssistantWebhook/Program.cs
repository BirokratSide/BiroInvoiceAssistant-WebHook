using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using si.birokrat.next.common.build;
using si.birokrat.next.common.logging;

namespace InvoiceAssistantWebhook
{
    public class Program
    {
        private static string projectPath;
        private static string appSecretsPath;
        private static string appSecretsExamplePath;

        public static void Main(string[] args)
        {
            projectPath = Build.ProjectPath;
            appSecretsPath = Path.Combine(projectPath, "appsettings.secrets.json");
            appSecretsExamplePath = Path.Combine(projectPath, "appsettings.secrets.example.json");

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            VerifySecretsOrCopyExample(appSecretsPath, appSecretsExamplePath);
            var configuration = CompileConfigurationIntoObject(projectPath);
            Log.Logger = CompileLoggerConfiguration(configuration);

            return WebHost.CreateDefaultBuilder(args)
                   .UseStartup<Startup>()
                   .UseConfiguration(configuration)
                   .ConfigureLogging(builder =>
                   {
                       builder.ClearProviders();
                       builder.AddSerilog();
                   })
                   .UseKestrel(options => options.ConfigureEndpoints()) //TURN ON FOR HTTPS
                   .Build();
        }

        #region [auxiliary]
        private static void VerifySecretsOrCopyExample(string appSecretsPath, string appSecretsExamplePath)
        {
            if (!File.Exists(appSecretsPath))
            {
                File.Copy(appSecretsExamplePath, appSecretsPath);
                Logger.Log("Exception", "File 'appsettings.Secrets.Example.json' copied to 'appsettings.Secrets.json'. Verify settings and rerun. Press any key to exit.", toConsole: true);
                Console.ReadLine();
                Environment.Exit(-1);
            }
        }

        private static IConfigurationRoot CompileConfigurationIntoObject(string projectPath)
        {
            return new ConfigurationBuilder()
                .SetBasePath(projectPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Secrets.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }

        private static Serilog.Core.Logger CompileLoggerConfiguration(IConfigurationRoot configuration)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Override("Default", GetLogLevel(configuration["Logging:LogLevel:Default"]))
                .MinimumLevel.Override("System", GetLogLevel(configuration["Logging:LogLevel:System"]))
                .MinimumLevel.Override("Microsoft", GetLogLevel(configuration["Logging:LogLevel:Microsoft"]))
                .Enrich.FromLogContext()
                .WriteTo.File(Path.Combine(projectPath, "serilog.txt"))
                .WriteTo.Console(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();
        }

        private static LogEventLevel GetLogLevel(string level)
        {
            return (LogEventLevel)Enum.Parse(typeof(LogEventLevel), level);
        }
        #endregion
    }
}
