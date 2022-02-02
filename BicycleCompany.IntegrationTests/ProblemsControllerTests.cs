using AutoMapper;
using BicycleCompany.BLL;
using BicycleCompany.IntegrationTests.Utils;
using BicycleCompany.Models;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Response;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
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
    public class ProblemsControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>, IClassFixture<MapperFixture>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private readonly HttpClient _client;
        private readonly string _token;
        private readonly IMapper _mapper;

        public ProblemsControllerTests(CustomWebApplicationFactory<Startup> factory, MapperFixture mapperFixture)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            _mapper = mapperFixture.Mapper;

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
            var response = await _client.GetAsync("api/problems");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseProblemList = JsonConvert.DeserializeObject<List<ProblemForReadModel>>(responseString);
            var problemList = _mapper.Map<List<ProblemForReadModel>>(DbManager.GetSeedingProblems());
            
            responseProblemList = responseProblemList.Where(p => p.Description.Contains("Description")).ToList();

            response.Headers.GetValues("Pagination").Should().NotBeNull();
            responseProblemList.Should().NotBeNullOrEmpty();
            //responseProblemList.Should().BeEquivalentTo(problemList, "because they were initialized and doesn't changed");
        }

        [Fact]
        public async Task GetProblem_ShouldReturnOk_WithValidId()
        {
            // Arrange
            var response = await _client.GetAsync("api/problems");
            var responseString = await response.Content.ReadAsStringAsync();
            var responseProblem = JsonConvert.DeserializeObject<List<ProblemForReadModel>>(responseString)[0];
            var problemId = responseProblem.Id;

            // Act
            response = await _client.GetAsync("api/problems/" + problemId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().NotBeNull(responseString);
            var problem = JsonConvert.DeserializeObject<ProblemForReadModel>(responseString);
            problem.Should().BeEquivalentTo(responseProblem);
        }

        [Fact]
        public async Task GetProblem_ShouldReturnNotFound_WithInvalidId()
        {
            // Arrange
            var problemId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync("api/problems/" + problemId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateProblem_ShouldReturnOk_WithValidObject()
        {
            // Arrange
            var response = await _client.GetAsync("api/bicycles");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var bicycle = JsonConvert.DeserializeObject<List<BicycleForReadModel>>(responseString)[0];

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
                BicycleId = bicycle.Id,
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
                problemResponse.Bicycle.Should().BeEquivalentTo(bicycle);
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
        public async Task CreateProblem_ShouldReturnBadRequest_WithInvalidObject()
        {
            // Arrange
            var problem = new ProblemForCreateModel();
            var problemJson = JsonConvert.SerializeObject(problem);
            var data = new StringContent(problemJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("api/problems", data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreateProblem_ShouldReturnNotFound_WithInvalidBicycleId()
        {
            // Arrange
            var response = await _client.GetAsync("api/clients");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var client = JsonConvert.DeserializeObject<List<ClientForReadModel>>(responseString)[0];

            var problem = new ProblemForCreateModel()
            {
                BicycleId = Guid.NewGuid(),
                ClientId = client.Id,
                Description = "Test description",
                Place = "Test place",
            };
            var problemJson = JsonConvert.SerializeObject(problem);
            var data = new StringContent(problemJson, Encoding.UTF8, "application/json");

            // Act
            response = await _client.PostAsync("api/problems", data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("Bicycle");
        }

        [Fact]
        public async Task CreateProblem_ShouldReturnNotFound_WithInvalidClientId()
        {
            // Arrange
            var response = await _client.GetAsync("api/bicycles");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var bicycle = JsonConvert.DeserializeObject<List<BicycleForReadModel>>(responseString)[0];

            var problem = new ProblemForCreateModel()
            {
                BicycleId = bicycle.Id,
                ClientId = Guid.NewGuid(),
                Description = "Test description",
                Place = "Test place",
            };
            var problemJson = JsonConvert.SerializeObject(problem);
            var data = new StringContent(problemJson, Encoding.UTF8, "application/json");

            // Act
            response = await _client.PostAsync("api/problems", data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("Client");
        }

        [Fact]
        public async Task DeleteProblem_ShouldReturnNoContent_WithValidId()
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

        [Fact]
        public async Task DeleteProblem_ShouldReturnEntityNotFound_WithInvalidId()
        {
            // Arrange
            var problemId = Guid.NewGuid();

            // Act
            var response = await _client.DeleteAsync("api/problems/" + problemId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
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
        public async Task UpdateProblem_ShouldReturnNotFound_WithInvalidProblemId()
        {
            // Arrange
            var response = await _client.GetAsync("api/bicycles");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var bicyle = JsonConvert.DeserializeObject<List<BicycleForReadModel>>(responseString)[0];

            var problemId = Guid.NewGuid();

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
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("Problem");
        }

        [Fact]
        public async Task UpdateProblem_ShouldReturnNotFound_WithInvalidBicycleId()
        {
            // Arrange
            var response = await _client.GetAsync("api/problems");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var problemId = JsonConvert.DeserializeObject<List<ProblemForReadModel>>(responseString)[0].Id;

            var problemForUpdate = new ProblemForUpdateModel()
            {
                BicycleId = Guid.NewGuid(),
                Description = "New description",
                Place = "New Place",
                Stage = Stage.InProgress
            };

            var problemForUpdateJson = JsonConvert.SerializeObject(problemForUpdate);
            var data = new StringContent(problemForUpdateJson, Encoding.UTF8, "application/json");

            // Act
            response = await _client.PutAsync("api/problems/" + problemId, data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("Bicycle");
        }

        [Fact]
        public async Task PartiallyUpdateProblem_ShouldReturnNoContent_WithValidIdAndPatchDocument()
        {
            // Arrange
            const string Description = "patch description";
            const string Place = "patch place";
            const Stage Stage = Stage.OnTheWay;

            var response = await _client.GetAsync("api/bicycles");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var bicycle = JsonConvert.DeserializeObject<List<BicycleForReadModel>>(responseString)[0];
            var bicycleId = bicycle.Id;

            response = await _client.GetAsync("api/problems");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var problemId = JsonConvert.DeserializeObject<List<ProblemForReadModel>>(responseString)[0].Id;

            var operations = new List<Operation<ProblemForUpdateModel>>()
            {
                new Operation<ProblemForUpdateModel>("replace", "/description", null, Description),
                new Operation<ProblemForUpdateModel>("replace", "/place", null, Place),
                new Operation<ProblemForUpdateModel>("replace", "/stage", null, Stage),
                new Operation<ProblemForUpdateModel>("replace", "/bicycleId", null, bicycleId.ToString()),
            };
            var patchDoc = new JsonPatchDocument<ProblemForUpdateModel>(operations, new DefaultContractResolver());

            var patchDocJson = JsonConvert.SerializeObject(patchDoc);
            var data = new StringContent(patchDocJson, Encoding.UTF8, "application/json");

            // Act
            response = await _client.PatchAsync("api/problems/" + problemId, data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            response = await _client.GetAsync("api/problems/" + problemId);
            responseString = await response.Content.ReadAsStringAsync();
            var problemResponse = JsonConvert.DeserializeObject<ProblemForReadModel>(responseString);

            problemResponse.Should().NotBeNull();
            using (new AssertionScope())
            {
                problemResponse.Id.Should().Be(problemId);
                problemResponse.Bicycle.Id.Should().Be(bicycleId);
                problemResponse.Description.Should().Be(Description);
                problemResponse.Place.Should().Be(Place);
                problemResponse.Stage.Should().Be(Stage);
            }
        }

        [Fact]
        public async Task PartiallyUpdateProblem_ShouldReturnNotFound_WithInvalidId()
        {
            // Arrange
            var problemId = Guid.NewGuid();

            var patchDoc = new JsonPatchDocument<ProblemForUpdateModel>();

            var patchDocJson = JsonConvert.SerializeObject(patchDoc);
            var data = new StringContent(patchDocJson, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PatchAsync("api/problems/" + problemId, data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("Problem");
        }

        [Fact]
        public async Task PartiallyUpdateProblem_ShouldReturnBadRequest_WhenObjectNotValidAfterPatchDocument()
        {
            // Arrange
            var response = await _client.GetAsync("api/problems");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var problemId = JsonConvert.DeserializeObject<List<ProblemForReadModel>>(responseString)[0].Id;

            var operations = new List<Operation<ProblemForUpdateModel>>()
            {
                new Operation<ProblemForUpdateModel>("replace", "/stage", null, Stage.New),
                new Operation<ProblemForUpdateModel>("replace", "/description", null, new string('a', 1000)),
            };
            var patchDoc = new JsonPatchDocument<ProblemForUpdateModel>(operations, new DefaultContractResolver());

            var patchDocJson = JsonConvert.SerializeObject(patchDoc);
            var data = new StringContent(patchDocJson, Encoding.UTF8, "application/json");

            // Act
            response = await _client.PatchAsync("api/problems/" + problemId, data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task PartiallyUpdateProblem_ShouldReturnBadRequest_WhenPatchDocumentIsNull()
        {
            // Arrange
            var response = await _client.GetAsync("api/problems");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var problemId = JsonConvert.DeserializeObject<List<ProblemForReadModel>>(responseString)[0].Id;

            JsonPatchDocument<ProblemForUpdateModel> patchDoc = null;

            var patchDocJson = JsonConvert.SerializeObject(patchDoc);
            var data = new StringContent(patchDocJson, Encoding.UTF8, "application/json");

            // Act
            response = await _client.PatchAsync("api/problems/" + problemId, data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("patch document");
        }

        [Fact]
        public async Task GetPartListForProblem_ShouldReturnOk_WithValidProblemId()
        {
            // Arrange
            var response = await _client.GetAsync("api/bicycles");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var bicycle = JsonConvert.DeserializeObject<List<BicycleForReadModel>>(responseString)[0];

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
                BicycleId = bicycle.Id,
                ClientId = client.Id,
                Description = "Test description",
                Place = "Test place",
                Parts = parts.Select(p => new PartDetailsForCreateModel { PartId = p.Id, Amount = 1 }).ToList()
            };
            var problemJson = JsonConvert.SerializeObject(problem);
            var data = new StringContent(problemJson, Encoding.UTF8, "application/json");

            response = await _client.PostAsync("api/problems", data);
            response.StatusCode.Should().Be(HttpStatusCode.Created, "because POST should work correct");

            responseString = await response.Content.ReadAsStringAsync();
            var problemId = JsonConvert.DeserializeObject<AddedResponse>(responseString).Id;
            response = await _client.GetAsync("api/problems/" + problemId);
            responseString = await response.Content.ReadAsStringAsync();
            var problemResponse = JsonConvert.DeserializeObject<ProblemForReadModel>(responseString);

            // Act
            response = await _client.GetAsync($"api/problems/{problemId}/parts");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            responseString = await response.Content.ReadAsStringAsync();
            var responsePartList = JsonConvert.DeserializeObject<List<PartDetailsForReadModel>>(responseString);

            responsePartList.Select(pd => pd.Part.Id).ToList()
                .Should().BeEquivalentTo(parts.Select(p => p.Id));
        }

        [Fact]
        public async Task GetPartForProblem_ShouldReturnOk_WithValidProblemIdAndPartId()
        {
            // Arrange
            var response = await _client.GetAsync("api/bicycles");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var bicycle = JsonConvert.DeserializeObject<List<BicycleForReadModel>>(responseString)[0];

            response = await _client.GetAsync("api/clients");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var client = JsonConvert.DeserializeObject<List<ClientForReadModel>>(responseString)[0];

            response = await _client.GetAsync("api/parts");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var parts = JsonConvert.DeserializeObject<List<PartForReadModel>>(responseString)
                .Where(p => p.Name.Contains("Part"));
            var part = parts.FirstOrDefault();

            var problem = new ProblemForCreateModel()
            {
                BicycleId = bicycle.Id,
                ClientId = client.Id,
                Description = "Test description",
                Place = "Test place",
                Parts = parts.Select(p => new PartDetailsForCreateModel { PartId = p.Id, Amount = 1 }).ToList()
            };
            var problemJson = JsonConvert.SerializeObject(problem);
            var data = new StringContent(problemJson, Encoding.UTF8, "application/json");

            response = await _client.PostAsync("api/problems", data);
            response.StatusCode.Should().Be(HttpStatusCode.Created, "because POST should work correct");

            responseString = await response.Content.ReadAsStringAsync();
            var problemId = JsonConvert.DeserializeObject<AddedResponse>(responseString).Id;
            response = await _client.GetAsync("api/problems/" + problemId);
            responseString = await response.Content.ReadAsStringAsync();
            var problemResponse = JsonConvert.DeserializeObject<ProblemForReadModel>(responseString);

            // Act
            response = await _client.GetAsync($"api/problems/{problemId}/parts/{part.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().NotBeNull(responseString);
            var responsePartDetails = JsonConvert.DeserializeObject<PartDetailsForReadModel>(responseString);
            var responsePart = responsePartDetails.Part;
            responsePart.Should().BeEquivalentTo(part);
        }

        [Fact]
        public async Task GetPartForProblem_ShouldReturnNotFound_WithInvalidProblemId()
        {
            // Arrange
            var problemId = Guid.NewGuid();
            var partId = Guid.NewGuid();

            // Act
            var response = await _client.GetAsync($"api/problems/{problemId}/parts/{partId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("Problem");
        }

        [Fact]
        public async Task GetPartForProblem_ShouldReturnNotFound_WithInvalidPartId()
        {
            // Arrange
            var response = await _client.GetAsync("api/problems");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var problemId = JsonConvert.DeserializeObject<List<ProblemForReadModel>>(responseString)[0].Id;

            var partId = Guid.NewGuid();

            // Act
            response = await _client.GetAsync($"api/problems/{problemId}/parts/{partId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("Part");
        }

        [Fact]
        public async Task CreatePartForProblem_ShouldReturnCreated_WithValidProblemIdAndValidObject()
        {
            // Arrange
            var response = await _client.GetAsync("api/bicycles");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var bicycle = JsonConvert.DeserializeObject<List<BicycleForReadModel>>(responseString)[0];

            response = await _client.GetAsync("api/clients");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var client = JsonConvert.DeserializeObject<List<ClientForReadModel>>(responseString)[0];

            response = await _client.GetAsync("api/parts");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var parts = JsonConvert.DeserializeObject<List<PartForReadModel>>(responseString)
                .Where(p => p.Name.Contains("Part"));
            var part = parts.FirstOrDefault();
            var partDetails = new PartDetailsForCreateModel()
            {
                PartId = part.Id,
                Amount = 3
            };

            var problem = new ProblemForCreateModel()
            {
                BicycleId = bicycle.Id,
                ClientId = client.Id,
                Description = "Test description",
                Place = "Test place",
            };
            var problemJson = JsonConvert.SerializeObject(problem);
            var data = new StringContent(problemJson, Encoding.UTF8, "application/json");

            response = await _client.PostAsync("api/problems", data);
            response.StatusCode.Should().Be(HttpStatusCode.Created, "because POST should work correct");

            responseString = await response.Content.ReadAsStringAsync();
            var problemId = JsonConvert.DeserializeObject<AddedResponse>(responseString).Id;

            var partDetailJson = JsonConvert.SerializeObject(partDetails);
            data = new StringContent(partDetailJson, Encoding.UTF8, "application/json");

            // Act
            response = await _client.PostAsync($"api/problems/{problemId}/parts", data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            response = await _client.GetAsync($"api/problems/{problemId}/parts/{part.Id}");
            responseString = await response.Content.ReadAsStringAsync();

            var partDetailsResponse = JsonConvert.DeserializeObject<PartDetailsForReadModel>(responseString);

            partDetailsResponse.Should().NotBeNull();
            using (new AssertionScope())
            {
                partDetailsResponse.Amount.Should().Be(partDetails.Amount);
                partDetailsResponse.Part.Id.Should().Be(partDetails.PartId);
            }
        }

        [Fact]
        public async Task CreatePartForProblem_ShouldReturnNotFound_WithInvalidProblemId()
        {
            // Arrange
            var partDetails = new PartDetailsForCreateModel()
            {
                PartId = Guid.NewGuid(),
                Amount = 3
            };

            var partDetailJson = JsonConvert.SerializeObject(partDetails);
            var data = new StringContent(partDetailJson, Encoding.UTF8, "application/json");

            var problemId = Guid.NewGuid();

            // Act
            var response = await _client.PostAsync($"api/problems/{problemId}/parts", data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("Problem");
        }

        [Fact]
        public async Task CreatePartForProblem_ShouldReturnBadRequest_WithInvalidObject()
        {
            // Arrange
            var response = await _client.GetAsync("api/bicycles");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var bicycle = JsonConvert.DeserializeObject<List<BicycleForReadModel>>(responseString)[0];

            response = await _client.GetAsync("api/clients");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var client = JsonConvert.DeserializeObject<List<ClientForReadModel>>(responseString)[0];

            response = await _client.GetAsync("api/parts");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var parts = JsonConvert.DeserializeObject<List<PartForReadModel>>(responseString)
                .Where(p => p.Name.Contains("Part"));
            var part = parts.FirstOrDefault();
            var partDetails = new PartDetailsForCreateModel()
            {
                PartId = part.Id,
                Amount = 0
            };

            var problem = new ProblemForCreateModel()
            {
                BicycleId = bicycle.Id,
                ClientId = client.Id,
                Description = "Test description",
                Place = "Test place",
            };
            var problemJson = JsonConvert.SerializeObject(problem);
            var data = new StringContent(problemJson, Encoding.UTF8, "application/json");

            response = await _client.PostAsync("api/problems", data);
            response.StatusCode.Should().Be(HttpStatusCode.Created, "because POST should work correct");

            responseString = await response.Content.ReadAsStringAsync();
            var problemId = JsonConvert.DeserializeObject<AddedResponse>(responseString).Id;

            var partDetailJson = JsonConvert.SerializeObject(partDetails);
            data = new StringContent(partDetailJson, Encoding.UTF8, "application/json");

            // Act
            response = await _client.PostAsync($"api/problems/{problemId}/parts", data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task CreatePartForProblem_ShouldReturnBadRequest_WhenSamePartAlreadyAdded()
        {
            // Arrange
            var response = await _client.GetAsync("api/bicycles");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var bicycle = JsonConvert.DeserializeObject<List<BicycleForReadModel>>(responseString)[0];

            response = await _client.GetAsync("api/clients");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var client = JsonConvert.DeserializeObject<List<ClientForReadModel>>(responseString)[0];

            response = await _client.GetAsync("api/parts");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var parts = JsonConvert.DeserializeObject<List<PartForReadModel>>(responseString)
                .Where(p => p.Name.Contains("Part"));
            var part = parts.FirstOrDefault();
            var partDetails = new PartDetailsForCreateModel()
            {
                PartId = part.Id,
                Amount = 1
            };

            var problem = new ProblemForCreateModel()
            {
                BicycleId = bicycle.Id,
                ClientId = client.Id,
                Description = "Test description",
                Place = "Test place",
                Parts = new List<PartDetailsForCreateModel>
                {
                    new PartDetailsForCreateModel()
                    {
                        PartId = part.Id,
                        Amount = 2
                    }
                }
            };
            var problemJson = JsonConvert.SerializeObject(problem);
            var data = new StringContent(problemJson, Encoding.UTF8, "application/json");

            response = await _client.PostAsync("api/problems", data);
            response.StatusCode.Should().Be(HttpStatusCode.Created, "because POST should work correct");

            responseString = await response.Content.ReadAsStringAsync();
            var problemId = JsonConvert.DeserializeObject<AddedResponse>(responseString).Id;

            var partDetailJson = JsonConvert.SerializeObject(partDetails);
            data = new StringContent(partDetailJson, Encoding.UTF8, "application/json");

            // Act
            response = await _client.PostAsync($"api/problems/{problemId}/parts", data);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("Same");
        }

        [Fact]
        public async Task DeletePartForProblem_ShouldReturnNoContent_WithValidId()
        {
            // Arrange
            var response = await _client.GetAsync("api/bicycles");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var bicycle = JsonConvert.DeserializeObject<List<BicycleForReadModel>>(responseString)[0];

            response = await _client.GetAsync("api/clients");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var client = JsonConvert.DeserializeObject<List<ClientForReadModel>>(responseString)[0];

            response = await _client.GetAsync("api/parts");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var parts = JsonConvert.DeserializeObject<List<PartForReadModel>>(responseString)
                .Where(p => p.Name.Contains("Part"));
            var part = parts.FirstOrDefault();

            var problem = new ProblemForCreateModel()
            {
                BicycleId = bicycle.Id,
                ClientId = client.Id,
                Description = "Test description",
                Place = "Test place",
                Parts = parts.Select(p => new PartDetailsForCreateModel { PartId = p.Id, Amount = 1 }).ToList()
            };
            var problemJson = JsonConvert.SerializeObject(problem);
            var data = new StringContent(problemJson, Encoding.UTF8, "application/json");

            response = await _client.PostAsync("api/problems", data);
            response.StatusCode.Should().Be(HttpStatusCode.Created, "because POST should work correct");

            responseString = await response.Content.ReadAsStringAsync();
            var problemId = JsonConvert.DeserializeObject<AddedResponse>(responseString).Id;

            // Act
            response = await _client.DeleteAsync($"api/problems/{problemId}/parts/{part.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
            response = await _client.GetAsync($"api/problems/{problemId}/parts/{part.Id}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeletePartForProblem_ShouldReturnNotFound_WithInvalidProblemId()
        {
            // Arrange
            var problemId = Guid.NewGuid();
            var partId = Guid.NewGuid();

            // Act
            var response = await _client.DeleteAsync($"api/problems/{problemId}/parts/{partId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("Problem");
        }

        [Fact]
        public async Task DeletePartForProblem_ShouldReturnNotFound_WithInvalidPartId()
        {
            // Arrange
            var response = await _client.GetAsync("api/bicycles");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var bicycle = JsonConvert.DeserializeObject<List<BicycleForReadModel>>(responseString)[0];

            response = await _client.GetAsync("api/clients");
            response.EnsureSuccessStatusCode();
            responseString = await response.Content.ReadAsStringAsync();
            var client = JsonConvert.DeserializeObject<List<ClientForReadModel>>(responseString)[0];

            var problem = new ProblemForCreateModel()
            {
                BicycleId = bicycle.Id,
                ClientId = client.Id,
                Description = "Test description",
                Place = "Test place",
            };
            var problemJson = JsonConvert.SerializeObject(problem);
            var data = new StringContent(problemJson, Encoding.UTF8, "application/json");

            response = await _client.PostAsync("api/problems", data);
            response.StatusCode.Should().Be(HttpStatusCode.Created, "because POST should work correct");

            responseString = await response.Content.ReadAsStringAsync();
            var problemId = JsonConvert.DeserializeObject<AddedResponse>(responseString).Id;

            var partId = Guid.NewGuid();

            // Act
            response = await _client.DeleteAsync($"api/problems/{problemId}/parts/{partId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
            responseString = await response.Content.ReadAsStringAsync();
            responseString.Should().Contain("Part");
        }
    }
}
