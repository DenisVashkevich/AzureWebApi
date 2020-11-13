using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.ApplicationInsights;
using Microsoft.ApplicationInsights;
using Serilog;
using Microsoft.Extensions.Logging;

namespace AdventureWorks.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();

            try
			{
				Log.Information("Application start");
				CreateHostBuilder(args).Build().Run();
			}
			catch(Exception ex)
			{
				Log.Fatal(ex, "Application start-up failed");
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
				})
				.ConfigureLogging((hostingcontext, logging) =>
				{
					logging.AddApplicationInsights("b5ca8a28-74f0-4d18-8574-7883fbe349cc");
					logging.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Trace);
					logging.AddFilter<ApplicationInsightsLoggerProvider>("Microsoft", LogLevel.Warning);
				})
				.UseSerilog();
	}
}
