using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace BicycleCompany.IntegrationTests.Utils
{
    public class CustomWebApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup: class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d =>
                    d.ServiceType == typeof(DbContextOptions<RepositoryContext>));

                services.Remove(descriptor);

                services.AddDbContext<RepositoryContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<RepositoryContext>();
                    var logger = scopedServices.GetRequiredService<ILoggerManager>();

                    db.Database.EnsureCreated();

                    try
                    {
                        DbManager.InitializeDbForTests(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError($"{ex.Message}. An error occured seeding the database with test messages." +
                            $"{ex.Message}");
                    }
                }
            });
        }
    }
}
