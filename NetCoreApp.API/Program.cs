using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System;
using System.IO;
using ILogger = Serilog.ILogger;

namespace NetCoreApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.Logger = CreateLogger();
                CreateHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.ForContext<Program>().Fatal(ex, $"Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>

            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static ILogger CreateLogger()
        {
            var appConfiguration = GetAppConfiguration();

            var sinkOptions = new MSSqlServerSinkOptions { TableName = "Logs" };
            var columnOptions = new ColumnOptions();
            columnOptions.Id.DataType = System.Data.SqlDbType.BigInt;

            var loggerConfiguration = new LoggerConfiguration()
                .WriteTo.MSSqlServer(
                    connectionString: appConfiguration.GetConnectionString("SqlConnectionString"),
                    sinkOptions: sinkOptions,
                    columnOptions: columnOptions,
                    appConfiguration: appConfiguration
                );

            return loggerConfiguration.CreateLogger();
        }

        private static IConfiguration GetAppConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
    }
}
