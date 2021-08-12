using BicycleCompany.BLL.Controllers;
using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.BLL.Utils;
using BicycleCompany.Models;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Response;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
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
                    Stage = Stage.New,
                    Parts = new List<PartDetailsForReadModel>
                    {
                        new PartDetailsForReadModel()
                        {
                            Part = new PartForReadModel(),
                            Amount = 2
                        }
                    }
                },
                new ProblemForReadModel
                {
                    Id = new Guid("55CEDE3A-83CC-48C9-A41D-E094F35F3DF4"),
                    Bicycle = new BicycleForReadModel(),
                    Description = "Description",
                    Place = "Place 2",
                    ReceivingDate = new DateTime(2021, 7, 13),
                    Stage = Stage.New
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
            var response = await controller.GetProblemList(null);

            // Assert
            Assert.IsType<OkObjectResult>(response);

            var result = response as OkObjectResult;
            Assert.NotNull(result.Value);
            Assert.IsType<List<ProblemForReadModel>>(result.Value);
            var problems = result.Value as List<ProblemForReadModel>;

            problems.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetProblemList_ShouldReturnOk_WithData()
        {
            // Arrange
            var fakeProblemList = GetFakeProblemList();
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetProblemListAsync(null, null))
                .Returns(Task.FromResult(fakeProblemList));
            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            var response = await controller.GetProblemList(null);

            // Assert
            Assert.IsType<OkObjectResult>(response);
            var result = response as OkObjectResult;

            Assert.NotNull(result.Value);
            Assert.IsType<List<ProblemForReadModel>>(result.Value);
            var problems = result.Value as List<ProblemForReadModel>;

            problems.Should().HaveSameCount(fakeProblemList);
            problems.Should().BeEquivalentTo(fakeProblemList);
        }

        [Fact]
        public async Task GetProblem_ShouldReturnOk_WithValidId()
        {
            // Arrange
            var guid = new Guid("55CEDE3A-83CC-48C9-A41D-E094F35F3DF3");
            var problem = GetFakeProblemList().Find(p => p.Id.Equals(guid));

            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetProblemAsync(guid))
                .Returns(Task.FromResult(problem));

            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            var response = await controller.GetProblem(guid);

            // Assert
            Assert.IsType<OkObjectResult>(response);

            var result = response as OkObjectResult;
            Assert.NotNull(result.Value);
            Assert.IsType<ProblemForReadModel>(result.Value);

            var responseProblem = result.Value as ProblemForReadModel;
            Assert.Equal(problem.Id, responseProblem.Id);
            Assert.NotNull(responseProblem.Bicycle);
            Assert.Equal(problem.Bicycle.Id, responseProblem.Bicycle.Id);
            Assert.Equal(problem.Bicycle.Name, responseProblem.Bicycle.Name);
            Assert.Equal(problem.Bicycle.Model, responseProblem.Bicycle.Model);
            Assert.Equal(problem.Place, responseProblem.Place);
            Assert.Equal(problem.Stage, responseProblem.Stage);
            Assert.Equal(problem.Description, responseProblem.Description);
            Assert.Equal(problem.ReceivingDate, responseProblem.ReceivingDate);
            responseProblem.Parts.Should().BeEquivalentTo(problem.Parts);
        }

        [Fact]
        public async Task GetProblem_ShouldThrowEntityNotFoundException_WithInvalidId()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                .Throws<EntityNotFoundException>();

            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            Func<Task> act = async () => await controller.GetProblem(Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>();
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
        public async Task DeleteProblem_ShouldThrowEntityNotFoundException_WithInvalidId()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.DeleteProblemAsync(It.IsAny<Guid>()))
                .Throws<EntityNotFoundException>();

            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            Func<Task> act = async () => await controller.DeleteProblem(Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>();
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
        public async Task UpdateProblem_ShouldThrowEntityNotFoundException_WithInvalidId()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.UpdateProblemAsync(It.IsAny<Guid>(), It.IsAny<ProblemForUpdateModel>()))
                .Throws<EntityNotFoundException>();

            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            Func<Task> act = async () => await controller.UpdateProblem(Guid.NewGuid(), new ProblemForUpdateModel());

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task PartiallyUpdateProblem_ShouldReturnNoContent_WithValidIdAndPatchDoc()
        {
            // Arrange
            var problemForUpdate = new ProblemForUpdateModel()
            {
                BicycleId = Guid.NewGuid(),
                Description = "Desc",
                Place = "Place",
                Stage = Stage.InProgress
            };
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.UpdateProblemAsync(It.IsAny<Guid>(), It.IsAny<ProblemForUpdateModel>()));
            fakeProblemService.Setup(r => r.GetProblemForUpdateModelAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(problemForUpdate));

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                          It.IsAny<ValidationStateDictionary>(),
                                          It.IsAny<string>(),
                                          It.IsAny<Object>()));
            var controller = new ProblemsController(null, fakeProblemService.Object);
            controller.ObjectValidator = objectValidator.Object;

            // Act
            var response = await controller.PartiallyUpdateProblem(Guid.NewGuid(), new JsonPatchDocument<ProblemForUpdateModel>());

            // Assert
            Assert.IsType<NoContentResult>(response);
            fakeProblemService.Verify(r =>
                r.UpdateProblemAsync(It.IsAny<Guid>(), It.IsAny<ProblemForUpdateModel>()), Times.Once);
        }

        [Fact]
        public async Task PartiallyUpdateProblem_ShouldThrowEntityNotFoundException_WithInvalidId()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetProblemForUpdateModelAsync(It.IsAny<Guid>()))
                .Throws<EntityNotFoundException>();

            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            Func<Task> act = async () => await controller.PartiallyUpdateProblem(Guid.NewGuid(), new JsonPatchDocument<ProblemForUpdateModel>());

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task PartiallyUpdateProblem_ShouldThrowArgumentException_WithInvalidObjectAfterPatch()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetProblemForUpdateModelAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(new ProblemForUpdateModel()));

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(o => o.Validate(It.IsAny<ActionContext>(),
                                          It.IsAny<ValidationStateDictionary>(),
                                          It.IsAny<string>(),
                                          It.IsAny<Object>()));
            var controller = new ProblemsController(null, fakeProblemService.Object);
            controller.ObjectValidator = objectValidator.Object;
            controller.ModelState.AddModelError("Fields", "Required");

            // Act
            Func<Task> act = async () => await controller.PartiallyUpdateProblem(Guid.NewGuid(), new JsonPatchDocument<ProblemForUpdateModel>());

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task GetPartListForProblem_ShouldReturnOk_WithData()
        {
            // Arrange
            var fakePartList = GetFakePartList();
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetPartListForProblemAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(fakePartList));
            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            var response = await controller.GetPartListForProblem(Guid.NewGuid());

            // Assert
            Assert.IsType<OkObjectResult>(response);
            var result = response as OkObjectResult;

            Assert.NotNull(result.Value);
            Assert.IsType<List<PartDetailsForReadModel>>(result.Value);
            var problems = result.Value as List<PartDetailsForReadModel>;

            problems.Should().HaveSameCount(fakePartList);
            problems.Should().BeEquivalentTo(fakePartList);
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
            var response = await controller.GetPartListForProblem(Guid.NewGuid());

            // Assert
            Assert.IsType<OkObjectResult>(response);
            var result = response as OkObjectResult;

            Assert.NotNull(result.Value);
            Assert.IsType<List<PartDetailsForReadModel>>(result.Value);
            var problems = result.Value as List<PartDetailsForReadModel>;

            problems.Should().HaveCount(0);
        }

        [Fact]
        public async Task GetPartListForProblem_ShouldThrowEntityNotFoundException_WithInvalidProblemId()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetPartListForProblemAsync(It.IsAny<Guid>()))
                .Throws<EntityNotFoundException>();
            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            Func<Task> act = async () => await controller.GetPartListForProblem(Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetPartForProblem_ShouldReturnOk_WithValidIds()
        {
            // Arrange
            var fakePart = GetFakePartList().FirstOrDefault();
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetPartForProblemAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(fakePart));
            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            var response = await controller.GetPartForProblem(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            Assert.IsType<OkObjectResult>(response);
            var result = response as OkObjectResult;

            Assert.NotNull(result.Value);
            Assert.IsType<PartDetailsForReadModel>(result.Value);
            var part = result.Value as PartDetailsForReadModel;

            part.Should().BeEquivalentTo(fakePart);
        }

        [Fact]
        public async Task GetPartForProblem_ShouldThrowEntityNotFoundException_WithInvalidProblemId()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                .Throws<EntityNotFoundException>();
            fakeProblemService.Setup(r => r.GetPartForProblemAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Callback(() => fakeProblemService.Object.GetProblemAsync(Guid.NewGuid()));
            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            Func<Task> act = async () => await controller.GetPartForProblem(Guid.NewGuid(), It.IsAny<Guid>());

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task GetPartForProblem_ShouldThrowEntityNotFoundException_WithInvalidPartId()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetPartForProblemAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Throws<EntityNotFoundException>();
            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            Func<Task> act = async () => await controller.GetPartForProblem(Guid.NewGuid(), It.IsAny<Guid>());

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>();
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
        public async Task CreatePartForProblem_ShouldThrowEntityNotFoundException_WithInvalidProblemId()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                .Throws<EntityNotFoundException>();
            fakeProblemService.Setup(r => r.CreatePartForProblemAsync(It.IsAny<Guid>(), It.IsAny<PartDetailsForCreateModel>()))
                .Callback(() => fakeProblemService.Object.GetProblemAsync(Guid.NewGuid()));
            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            Func<Task> act = async () => await controller.CreatePartForProblem(Guid.NewGuid(), new PartDetailsForCreateModel());

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task CreatePartForProblem_ShouldThrowArgumentException_WhenPartAlreadyAdded()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.CreatePartForProblemAsync(It.IsAny<Guid>(), It.IsAny<PartDetailsForCreateModel>()))
                .Throws<ArgumentException>();
            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            Func<Task> act = async () => await controller.CreatePartForProblem(Guid.NewGuid(), new PartDetailsForCreateModel());

            // Assert
            await act.Should().ThrowAsync<ArgumentException>();
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

        [Fact]
        public async Task DeletePartForProblem_ShouldThrowEntityNotFoundException_WithInvalidProblemId()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                .Throws<EntityNotFoundException>();
            fakeProblemService.Setup(r => r.DeletePartForProblemAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Callback(() => fakeProblemService.Object.GetProblemAsync(Guid.NewGuid()));

            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            Func<Task> act = async () => await controller.DeletePartForProblem(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task DeletePartForProblem_ShouldThrowEntityNotFoundException_WithInvalidPartId()
        {
            // Arrange
            var fakeProblemService = new Mock<IProblemService>();
            fakeProblemService.Setup(r => r.DeletePartForProblemAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Throws<EntityNotFoundException>();

            var controller = new ProblemsController(null, fakeProblemService.Object);

            // Act
            Func<Task> act = async () => await controller.DeletePartForProblem(Guid.NewGuid(), Guid.NewGuid());

            // Assert
            await act.Should().ThrowAsync<EntityNotFoundException>();
        }
    }
}