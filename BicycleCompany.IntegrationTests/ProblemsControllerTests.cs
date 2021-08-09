using BicycleCompany.BLL;
using BicycleCompany.IntegrationTests.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xunit;

namespace BicycleCompany.IntegrationTests
{
    public class ProblemsControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;

        public ProblemsControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Test1()
        {
            //var request = new HttpRequestMessage(HttpMethod.Post, "/api/authentication/login");
            //var response = await _client.PostAsync("/api/authentication/login", );
            //response.EnsureSuccessStatusCode();
            //var responseString = await response.Content.ReadAsStringAsync();

            //Assert.NotEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
