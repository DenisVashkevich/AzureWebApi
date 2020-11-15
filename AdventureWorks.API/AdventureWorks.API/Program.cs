using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.ApplicationInsights;using Serilog;
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
				.WriteTo.ApplicationInsights("b5ca8a28-74f0-4d18-8574-7883fbe349cc", TelemetryConverter.Events)
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
				.UseSerilog();
	}
}
