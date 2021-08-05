using AutoMapper;
using BicycleCompany.BLL.Controllers;
using BicycleCompany.BLL.Mapping;
using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.BLL.Utils;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BicycleCompany.Tests
{
    public class ProblemsControllerTests
    {
        private List<ProblemForReadModel> GetFakeProblemList()
        {
            var list = new List<ProblemForReadModel>
            {
                new ProblemForReadModel
                {
                    Id = new Guid("55CEDE3A-83CC-48C9-A41D-E094F35F3DF3"),
                    Bicycle = new BicycleForReadModel(),
                    Description = "Very well",
                    Place = "Place",
                    ReceivingDate = new DateTime(2021, 7, 12),
                    Stage = 0
                },
                new ProblemForReadModel
                {
                    Id = Guid.NewGuid(),
                    Bicycle = new BicycleForReadModel(),
                    Description = "Description",
                    Place = "Place 2",
                    ReceivingDate = new DateTime(2021, 7, 13),
                    Stage = 1
                }
            };

            return list;
        }

        private List<PartDetailsForReadModel> GetFakePartList()
        {
            var list = new List<PartDetailsForReadModel>
            {
                new PartDetailsForReadModel
                {
                    Part = new PartForReadModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Wheel"
                    },
                    Amount = 1
                },
                new PartDetailsForReadModel
                {
                    Part = new PartForReadModel
                    {
                        Id = Guid.NewGuid(),
                        Name = "Chain"
                    },
                    Amount = 1
                }
            };

            return list;
        }

        [Fact]
        public async Task GetProblemList_ShouldReturnOk_NoData()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetProblemListAsync(null, null))
                .Returns(Task.FromResult(new List<ProblemForReadModel>()));
            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            var result = await controller.GetProblemList(null);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetProblemList_ShouldReturnOk_WithData()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetProblemListAsync(null, null))
                .Returns(Task.FromResult(GetFakeProblemList()));
            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            var response = await controller.GetProblemList(null);

            // Assert
            Assert.IsType<OkObjectResult>(response);
            var result = response as OkObjectResult;

            Assert.IsType<List<ProblemForReadModel>>(result.Value);
            var problems = result.Value as List<ProblemForReadModel>;

            Assert.Equal(problems.Count, GetFakeProblemList().Count);
        }

        [Fact]
        public async Task GetProblem_ShouldReturnOk_WithValidId()
        {
            // Arrange
            var guid = new Guid("55CEDE3A-83CC-48C9-A41D-E094F35F3DF3");

            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetProblemAsync(guid))
                .Returns(Task.FromResult(GetFakeProblemList()
                             .Find(p => p.Id.Equals(guid))));

            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            var response = await controller.GetProblem(guid);

            // Assert
            Assert.IsType<OkObjectResult>(response);

            var result = response as OkObjectResult;
            Assert.IsType<ProblemForReadModel>(result.Value);
        }

        [Fact]
        public async Task CreateProblem_ShouldReturnCreated_WithValidObject()
        {
            // Arrange
            Guid problemId = Guid.NewGuid();
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.CreateProblemAsync(It.IsAny<ProblemForCreateModel>()))
                .Returns(Task.FromResult(problemId));

            var request = new ProblemForCreateModel();
            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            var response = await controller.CreateProblem(request);

            // Assert
            Assert.IsType<CreatedAtRouteResult>(response);
            var result = response as CreatedAtRouteResult;

            Assert.IsType<AddedResponse>(result.Value);
            var addedResult = result.Value as AddedResponse;
            Assert.Equal(problemId, addedResult.Id);
        }

        [Fact]
        public async Task CreateProblem_ShouldThrowArgumentException_WithInvalidObject()
        {
            // Arrange
            var controller = new ProblemsController(null, null);
            controller.ModelState.AddModelError("Fields", "Required");

            var problem = new ProblemForCreateModel();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => 
                await controller.CreateProblem(problem));
        }

        [Fact]
        public async Task DeleteProblem_ShouldReturnNoContent_WithValidId()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.DeleteProblemAsync(It.IsAny<Guid>()));

            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            var response = await controller.DeleteProblem(Guid.NewGuid());

            // Assert
            Assert.IsType<NoContentResult>(response);
            fakeProblemService.Verify(r =>
                r.DeleteProblemAsync(It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProblem_ShouldReturnNoContent_WithValidObject()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.UpdateProblemAsync(It.IsAny<Guid>(), It.IsAny<ProblemForUpdateModel>()));

            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            var response = await controller.UpdateProblem(Guid.NewGuid(), new ProblemForUpdateModel());

            // Assert
            Assert.IsType<NoContentResult>(response);
            fakeProblemService.Verify(r => 
                r.UpdateProblemAsync(It.IsAny<Guid>(), It.IsAny<ProblemForUpdateModel>()), Times.Once);
        }

        [Fact]
        public async Task UpdateProblem_ShouldThrowArgumentException_WithInvalidObject()
        {
            // Arrange
            var controller = new ProblemsController(null, null);
            controller.ModelState.AddModelError("Fields", "Required");

            var problem = new ProblemForUpdateModel();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await controller.UpdateProblem(Guid.NewGuid(), problem));
        }

        [Fact]
        public async Task GetPartListForProblem_ShouldReturnOk_WithData()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetPartListForProblemAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(GetFakePartList()));
            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            var response = await controller.GetPartListForProblem(Guid.NewGuid());

            // Assert
            Assert.IsType<OkObjectResult>(response);
            var result = response as OkObjectResult;

            Assert.IsType<List<PartDetailsForReadModel>>(result.Value);
            var problems = result.Value as List<PartDetailsForReadModel>;

            Assert.Equal(problems.Count, GetFakeProblemList().Count);
        }

        [Fact]
        public async Task GetPartListForProblem_ShouldReturnOk_NoData()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetPartListForProblemAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new List<PartDetailsForReadModel>()));
            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            var result = await controller.GetPartListForProblem(Guid.NewGuid());

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreatePartForProblem_ShoulReturnCreated_WithValidObject()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.CreatePartForProblemAsync(It.IsAny<Guid>(), It.IsAny<PartDetailsForCreateModel>()))
                .Returns(Task.FromResult(guid));
            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            var response = await controller.CreatePartForProblem(Guid.NewGuid(), new PartDetailsForCreateModel());

            // Assert
            Assert.IsType<CreatedAtRouteResult>(response);
            var result = response as CreatedAtRouteResult;

            Assert.IsType<AddedResponse>(result.Value);
            var addedResult = result.Value as AddedResponse;
            Assert.Equal(guid, addedResult.Id);
        }

        [Fact]
        public async Task CreatePartForProblem_ShoulThrowArgumentException_WithInvalidObject()
        {
            // Arrange
            var controller = new ProblemsController(null, null);
            controller.ModelState.AddModelError("Fields", "Required");

            var part = new PartDetailsForCreateModel();

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await controller.CreatePartForProblem(Guid.NewGuid(), part));
        }

        [Fact]
        public async Task DeletePartForProblem_ShouldReturnNoContent_WithValidId()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.DeletePartForProblemAsync(It.IsAny<Guid>(), It.IsAny<Guid>()));

            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            var response = await controller.DeletePartForProblem(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            Assert.IsType<NoContentResult>(response);
            fakeProblemService.Verify(r =>
                r.DeletePartForProblemAsync(It.IsAny<Guid>(), It.IsAny<Guid>()), Times.Once);
        }
    }
}