using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShortenUrl.API.Controllers;
using ShortenUrl.API.Data;
using ShortenUrl.API.Data.Interfaces;
using ShortenUrl.API.Logic;
using System;

namespace ShortenUrl.Tests
{
    public class FakeStartup
    {
        private readonly InMemoryDatabaseRoot _inMemory = new();
        public FakeStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlite().AddDbContext<AppDbContext>();
            //services.AddDbContext<AppDbContext>(options =>
            //{
            //    options.UseInMemoryDatabase("InMemoryTestDb", _inMemory);
            //});
            services.AddScoped<IShortenUrlLogic, ShortenUrlLogic>();
            services.AddMvc()
                .AddApplicationPart(typeof(ShortenUrlController).Assembly);
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Use(async (context, next) =>
            {
                if (string.IsNullOrEmpty(context.Request.Cookies["userId"]) || string.IsNullOrEmpty(context.Request.Cookies["sessionId"]))
                {
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/json";
                    return;
                }
                await next();
            });

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using var serviceScope = serviceScopeFactory.CreateScope();
            var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
            if (dbContext == null)
            {
                throw new NullReferenceException("Cannot get instance of dbContext");
            }

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();
        }
    }
}
