using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ShortenUrl.API.Data;
using ShortenUrl.API.Data.Interfaces;
using ShortenUrl.API.Logic;

namespace ShortenUrl.API
{
    public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddEntityFrameworkSqlite().AddDbContext<AppDbContext>();

			services.AddControllers();
			services.AddScoped<IShortenUrlLogic, ShortenUrlLogic>();
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShortenUrl.API", Version = "v1" });
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShortenUrl.API v1"));
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.Use(async(context, next) =>
            {
				if(string.IsNullOrEmpty(context.Request.Cookies["userId"]) || string.IsNullOrEmpty(context.Request.Cookies["sessionId"]))
                {
					context.Response.StatusCode = 401;
					context.Response.ContentType = "application/json";
					return;
                }
				await next();
            });

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
