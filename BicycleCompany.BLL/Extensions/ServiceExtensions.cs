using BicycleCompany.BLL.Services;
using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.DAL;
using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;

namespace BicycleCompany.BLL.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<RepositoryContext>(opts =>
                opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));

        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddScoped<ILoggerManager, LoggerManager>();

        public static void RegisterRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBicycleRepository, BicycleRepository>();
            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<IPartRepository, PartRepository>();
            services.AddScoped<IProblemRepository, ProblemRepository>();
            services.AddScoped<IPartProblemRepository, PartProblemRepository>();
        }

        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IBicycleService, BicycleService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IPartService, PartService>();
            services.AddScoped<IProblemService, ProblemService>();
        }

        public static void ConfigureSwagger(this IServiceCollection services) =>
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "BicycleCompany API",
                    Description = "A simple ASP.NET Core Web API",
                    Contact = new OpenApiContact
                    {
                        Name = "Andrew Vertuha",
                        Email = string.Empty,
                        Url = new Uri("https://localhost:5001/swagger/index.html"),
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
    }
}
