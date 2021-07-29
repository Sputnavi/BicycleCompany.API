using BicycleCompany.BLL.Extensions;
using BicycleCompany.BLL.Services;
using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.BLL.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System.IO;

namespace BicycleCompany.BLL
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureSqlContext(Configuration);
            services.ConfigureLoggerService();
            services.AddAutoMapper(typeof(Startup));

            services.RegisterRepositories();
            services.RegisterServices();
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();

            services.AddAuthentication();
            services.ConfigureJwt(Configuration);
            services.AddRazorPages();
            services.AddControllers()
                .AddNewtonsoftJson();
            services.ConfigureSwagger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BicycleCompany v1"));
            }

            app.UseMiddleware<ExceptionHandler>();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=index}/{id?}");
            });
        }
    }
}
