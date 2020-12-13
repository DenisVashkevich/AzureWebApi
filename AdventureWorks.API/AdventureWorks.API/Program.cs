using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Serilog;
using System.IO;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

namespace AdventureWorks.API
{
	public class Program
	{
		private const string STORAGE_ACC_KEY_SECTION = "StorageAccount:ConnectionKey";
		private const string STORAGE_ACC_NAME_SECTION = "StorageAccount:AccountName";
		private const string SETTINGS_FILE_NAME = "appsettings.json";
		private const string APPCONFIG_SECTION = "AppConfig";

		public static void Main(string[] args)
		{
			var settings = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile(SETTINGS_FILE_NAME)
				.Build();

			var storage = new CloudStorageAccount(
				storageCredentials: new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
					accountName: settings[STORAGE_ACC_NAME_SECTION],
					keyValue: settings[STORAGE_ACC_KEY_SECTION]),
				useHttps: true
				);

			Log.Logger = new LoggerConfiguration()
				.WriteTo.AzureTableStorage(storage)
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
					webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
					{
						var settings = config.Build();
						config.AddAzureAppConfiguration(options =>
							options
								.Connect(settings.GetConnectionString(APPCONFIG_SECTION))
								.Select(KeyFilter.Any, LabelFilter.Null)
								.Select(KeyFilter.Any, hostingContext.HostingEnvironment.EnvironmentName)
						);
					}).UseStartup<Startup>();
				}).UseSerilog();
	}
}
