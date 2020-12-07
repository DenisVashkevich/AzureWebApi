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
using AutoMapper;
using AdventureWorks.API.Mappings;
using System;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerUI;
using AdventureWorks.DocStorage.Interfaces;
using AdventureWorks.DocStorage.Services;


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
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "AdventureWorksApi",
					Description = "REST API for DB CRUD operations",
					TermsOfService = new Uri("https://example.com/terms"),
					Contact = new OpenApiContact
					{
						Name = "Denis",
						Email = string.Empty,
						Url = new Uri("https://twitter.com/spboyer"),
					},
					License = new OpenApiLicense
					{
						Name = "Use under LICX",
						Url = new Uri("https://example.com/license"),
					}
				});
				c.CustomOperationIds(apiDesc =>
				{
					return apiDesc.TryGetMethodInfo(out MethodInfo methodInfo) ? methodInfo.Name : null;
				});
			});
			services.AddSwaggerGenNewtonsoftSupport();
			services.AddDbContext<ProductContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Entities")));
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IDocumentStorageService, DocumentBlobStorageService>();
			services.AddScoped<IUploadNotificationService, AzureQueueNotificationService>();
			services.AddAutoMapper(config =>
            {
				config.AddProfile(new MappingProfile());

            });
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
			app.UseEndpoints(options =>
			{
				options.MapControllers();
			});
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
				c.DefaultModelExpandDepth(2);
				c.DefaultModelRendering(ModelRendering.Model);
				c.DefaultModelsExpandDepth(-1);
				c.DisplayOperationId();
				c.DisplayRequestDuration();
				c.DocExpansion(DocExpansion.None);
				c.EnableDeepLinking();
				c.EnableFilter();
				c.MaxDisplayedTags(5);
				c.ShowExtensions();
				c.ShowCommonExtensions();
				c.EnableValidator();
				c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Head);
			});

		}
	}
}