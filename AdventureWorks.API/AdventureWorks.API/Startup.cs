using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.DbModel.Context;
using AdventureWorks.DbModel.Services;
using AdventureWorks.DbModel.Interfaces;
using Serilog;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;

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
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
			});
			services.AddDbContext<ProductContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Entities")));
			services.AddScoped<IProductService, ProductService>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
            }

			app.UseExceptionHandler("/Error");
			
			app.UseSerilogRequestLogging();
			app.UseRouting();
			app.UseCors(options =>
			{
				options.AllowAnyHeader();
				options.AllowAnyMethod();
				options.AllowAnyOrigin();
			});

			app.UseSwagger();

			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
			});

		}
	}
}