using Microsoft.AspNetCore.Blazor.Builder;
using Microsoft.Extensions.DependencyInjection;
using DotnetEFRazorCompSSR.App.Services;
using DotnetEFRazorCompSSR.App.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetEFRazorCompSSR.App
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebsitesContext>(options =>
                    options.UseInMemoryDatabase()                                // For In memory
                    //options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=DotnetEFRazorCompDb;Trusted_Connection=True;MultipleActiveResultSets=true"
                    //, b => b.MigrationsAssembly("DotnetEFRazorCompSSR.Server"))    // For MSSql
                , ServiceLifetime.Singleton
            );

            services.AddSingleton<WebsiteDataService>();
            //services.AddMvcCore().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Since Blazor is running on the server, we can use an application service
            // to read the forecast data.
            services.AddSingleton<WeatherForecastService>();
        }

        public void Configure(IBlazorApplicationBuilder app, WebsitesContext context)
        {
            //Populate initial data
            context.SeedData().GetAwaiter().GetResult();

            app.AddComponent<App>("app");
        }
    }
}
