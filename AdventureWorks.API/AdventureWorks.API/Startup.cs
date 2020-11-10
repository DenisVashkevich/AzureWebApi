using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.DbModel.Context;
using Serilog;
using Microsoft.Data.SqlClient;

namespace AdventureWorks.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();
			services.AddDbContext<ProductContext>(options => options.UseSqlServer(BuildConnectionString()));

		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseSerilogRequestLogging();
			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}

		private string BuildConnectionString()
        {
			var builder = new SqlConnectionStringBuilder(Configuration.GetConnectionString("Entities"));
			builder.Password = Configuration["DbPassword"];
			builder.UserID = Configuration["DbUserId"];
			return builder.ConnectionString;
		}
	}

}
