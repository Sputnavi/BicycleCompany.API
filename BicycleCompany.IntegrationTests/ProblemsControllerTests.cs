using AutoMapper;
using BicycleCompany.BLL;
using BicycleCompany.BLL.Mapping;
using BicycleCompany.IntegrationTests.Utils;
using BicycleCompany.Models;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Response;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BicycleCompany.IntegrationTests
{
    public class ProblemsControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;
        private readonly string _token;
        private readonly IMapper _mapper;

        public ProblemsControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var mappingConfiguration = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiles());
            });
            _mapper = mappingConfiguration.CreateMapper();

            // Authorization to set header.
            var user = new UserForAuthenticationModel()
            {
                Login = "Admin1",
                Password = "Admin"
            };
            var userJson = JsonConvert.SerializeObject(user);
            var data = new StringContent(userJson, Encoding.UTF8, "application/json");

            var response = _client.PostAsync("api/authentication/login", data).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;

            _token = JsonConvert.DeserializeObject<TokenResponseModel>(responseString).Token;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        [Fact]
        public async Task GetProblemList_ShouldReturnOk_WithData()
        {
            // Act
            var response = await _client.GetAsync("api/bicycles");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseBicycleList = JsonConvert.DeserializeObject<List<BicycleForReadModel>>(responseString);
            var bicycleList = _mapper.Map<List<BicycleForReadModel>>(DbManager.GetSeedingBicycles());
            
            responseBicycleList = responseBicycleList.Where(b => b.Name.Contains("Bicycle")).ToList();

            response.Headers.GetValues("Pagination").Should().NotBeNull();
            responseBicycleList.Should().NotBeNullOrEmpty();
            responseBicycleList.Should().BeEquivalentTo(bicycleList, "because they were initialized and doesn't changed");
        }

        [Fact]
        public async Task CreateProblem_ShouldReturnOk_WithValidObject()
        {
            // Arrange
            var response = await _client.GetAsync("api/bicycles");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var bicyle = JsonConvert.DeserializeObject<List<BicycleForReadModel>>(responseString)[0];

            response = await _client.GetAsync("api/clients");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var client = JsonConvert.DeserializeObject<List<ClientForReadModel>>(responseString)[0];

            response = await _client.GetAsync("api/parts");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var parts = JsonConvert.DeserializeObject<List<PartForReadModel>>(responseString)
                .Where(p => p.Name.Contains("Part"));

            var problem = new ProblemForCreateModel()
            {
                BicycleId = bicyle.Id,
                ClientId = client.Id,
                Description = "Test description",
                Place = "Test place",
                Parts = parts.Select(p => new PartDetailsForCreateModel { PartId = p.Id, Amount = 1 }).ToList()
            };
            var problemJson = JsonConvert.SerializeObject(problem);
            var data = new StringContent(problemJson, Encoding.UTF8, "application/json");

            // Act
            response = await _client.PostAsync("api/problems", data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            responseString = await response.Content.ReadAsStringAsync();

            var problemId = JsonConvert.DeserializeObject<AddedResponse>(responseString).Id;
            response = await _client.GetAsync("api/problems/" + problemId);
            responseString = await response.Content.ReadAsStringAsync();
            var problemResponse = JsonConvert.DeserializeObject<ProblemForReadModel>(responseString);

            problemResponse.Should().NotBeNull();
            using (new AssertionScope())
            {
                problemResponse.Id.Should().Be(problemId);
                problemResponse.Bicycle.Should().BeEquivalentTo(bicyle);
                problemResponse.Stage.Should().Be(Stage.New);
                problemResponse.Description.Should().Be(problem.Description);
                problemResponse.Place.Should().Be(problem.Place);
                problemResponse.ReceivingDate.Should().BeSameDateAs(DateTime.Today);
                problemResponse.Parts
                    .Select(p => p.Part.Id).ToList()
                    .Should().BeEquivalentTo(problem.Parts.Select(p => p.PartId));
            }
        }

        [Fact]
        public async Task UpdateProblem_ShouldReturnNoContent_WithValidObject()
        {
            // Arrange
            var response = await _client.GetAsync("api/bicycles");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var bicyle = JsonConvert.DeserializeObject<List<BicycleForReadModel>>(responseString)[0];

            response = await _client.GetAsync("api/problems");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var problemId = JsonConvert.DeserializeObject<List<ProblemForReadModel>>(responseString)[0].Id;

            var problemForUpdate = new ProblemForUpdateModel()
            {
                BicycleId = bicyle.Id,
                Description = "New description",
                Place = "New Place",
                Stage = Stage.InProgress
            };

            var problemForUpdateJson = JsonConvert.SerializeObject(problemForUpdate);
            var data = new StringContent(problemForUpdateJson, Encoding.UTF8, "application/json");

            // Act
            response = await _client.PutAsync("api/problems/" + problemId, data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            response = await _client.GetAsync("api/problems/" + problemId);
            responseString = await response.Content.ReadAsStringAsync();
            var problemResponse = JsonConvert.DeserializeObject<ProblemForReadModel>(responseString);

            problemResponse.Should().NotBeNull();
            using (new AssertionScope())
            {
                problemResponse.Id.Should().Be(problemId);
                problemResponse.Bicycle.Id.Should().Be(problemForUpdate.BicycleId);
                problemResponse.Description.Should().Be(problemForUpdate.Description);
                problemResponse.Place.Should().Be(problemForUpdate.Place);
                problemResponse.Stage.Should().Be(problemForUpdate.Stage);
            }
        }

        [Fact]
        public async Task DeleteProblem_ReturnNoContent_WithValidId()
        {
            // Arrange
            var response = await _client.GetAsync("api/problems");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var problemId = JsonConvert.DeserializeObject<List<ProblemForReadModel>>(responseString)[0].Id;

            // Act
            response = await _client.DeleteAsync("api/problems/" + problemId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            
            response = await _client.GetAsync("api/problems/" + problemId);
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
