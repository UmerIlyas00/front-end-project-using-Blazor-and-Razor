using DotnetEFRazorCompSSR.App.Models;
using DotnetEFRazorCompSSR.App.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DotnetEFRazorCompSSR.Tests
{
    public class ServiceTests
    {
        private WebsiteDataService websiteService;
        private WebsitesContext testContext;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddDbContext<WebsitesContext>(options => options.UseInMemoryDatabase(), ServiceLifetime.Singleton);
            services.AddSingleton<WebsiteDataService>();
            var serviceProvider = services.BuildServiceProvider();
            testContext = serviceProvider.GetService<WebsitesContext>();
            testContext.SeedData().GetAwaiter().GetResult();
            websiteService = new WebsiteDataService(testContext);
        }

        [Test]
        public void WebsiteDataService_GetWebsitesAsync()
        {
            var result = websiteService.GetWebsitesAsync(null,null).GetAwaiter().GetResult();
            Assert.True(result.Count > 0);
        }

        [Test]
        public void WebsiteDataService_GetMinMaxDateAsync()
        {
            var result = websiteService.GetMinMaxDateAsync().GetAwaiter().GetResult();
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.MaxDate);
            Assert.IsNotNull(result.MinDate);
        }
    }
}