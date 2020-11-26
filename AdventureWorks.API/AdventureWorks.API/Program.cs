using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.WindowsAzure.Storage;
using Serilog;

namespace AdventureWorks.API
{
	public class Program
	{
        private const string STORAGE_ACC_KEY_VALUE = "YXLMoq5Ie6ubryNjJxc9SrOV/9e8qT70VrPpzBafC6tdqzAPNm7JS8DcXi0rG1c6KKanmSZP8goGtf2Iaa8kxA==";
		private const string STORAGE_ACC_NAME = "advworksstorage";

        public static void Main(string[] args)
		{
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

			var storage = new CloudStorageAccount(
				storageCredentials: new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(accountName: STORAGE_ACC_NAME, keyValue: STORAGE_ACC_KEY_VALUE),
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
					webBuilder.UseStartup<Startup>();
				})
				.UseSerilog();
	}
}
