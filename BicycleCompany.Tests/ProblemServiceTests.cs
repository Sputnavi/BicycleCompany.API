using AutoMapper;
using BicycleCompany.BLL.Mapping;
using BicycleCompany.BLL.Services;
using BicycleCompany.BLL.Services.Contracts;
using BicycleCompany.BLL.Utils;
using BicycleCompany.DAL.Contracts;
using BicycleCompany.DAL.Models;
using BicycleCompany.Models.Request;
using BicycleCompany.Models.Request.RequestFeatures;
using BicycleCompany.Models.Response;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BicycleCompany.Tests
{
    [TestFixture]
    public class ProblemServiceTests
    {
        private IMapper _mapper;
        private ILoggerManager _loggerStub;

        [OneTimeSetUp]
        public void ConfigureCommon()
        {
            var mappingConfiguration = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfiles());
            });
            _mapper = mappingConfiguration.CreateMapper();
            
            var loggerStub = new Mock<ILoggerManager>();
            loggerStub.Setup(l => l.LogInfo(It.IsAny<string>()));
            _loggerStub = loggerStub.Object;
        }

        public PagedList<Problem> GetProblemList(bool hasData)
        {
            var list = new List<Problem>();
            if (hasData)
            {
                list.Add(
                    new Problem()
                    {
                        Id = Guid.NewGuid(),
                        BicycleId = Guid.NewGuid(),
                        ClientId = Guid.NewGuid(),
                        Description = "Description",
                        Place = "Place",
                        ReceivingDate = new DateTime(2021, 7, 12)
                    });
                list.Add(
                    new Problem
                    {
                        Id = Guid.NewGuid(),
                        BicycleId = Guid.NewGuid(),
                        ClientId = Guid.NewGuid(),
                        Description = "Description 2",
                        Place = "Place 2",
                        ReceivingDate = new DateTime(2021, 7, 13),
                    });
            }

            return PagedList<Problem>.ToPagedList(list, 1, list.Count);
        }

        public List<PartDetails> GetPartDetailsList(bool hasData)
        {
            var list = new List<PartDetails>();
            if (hasData)
            {
                Guid problemId = Guid.NewGuid();
                list.Add(
                    new PartDetails()
                    {
                        Id = Guid.NewGuid(),
                        PartId = Guid.NewGuid(),
                        ProblemId = problemId,
                        Amount = 3
                    });
                list.Add(
                    new PartDetails()
                    {
                        Id = Guid.NewGuid(),
                        PartId = Guid.NewGuid(),
                        ProblemId = problemId,
                        Amount = 7
                    });
            }

            return list;
        }

        [Test]
        public async Task GetProblemListAsync_ShouldReturnPagedProblemList_WithData()
        {
            // Arrange
            var problemList = GetProblemList(hasData: true);

            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemListAsync(null))
                    .Returns(Task.FromResult(problemList));
            
            var problemService = new ProblemService(fakeProblemRepository.Object, 
                null, null, null, _mapper, null, null);

            // Act
            var result = await problemService.GetProblemListAsync(null, null);

            // Assert
            Assert.IsInstanceOf<List<ProblemForReadModel>>(result);
            Assert.AreEqual(problemList.Count, result.Count);
        }

        [Test]
        public async Task GetProblemListAsync_ShouldReturnPagedProblemList_NoData()
        {
            // Arrange
            var problemList = GetProblemList(hasData: false);
            
            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemListAsync(null))
                    .Returns(Task.FromResult(problemList));

            var problemService = new ProblemService(fakeProblemRepository.Object,
                null, null, null, _mapper, null, null);

            // Act
            var result = await problemService.GetProblemListAsync(null, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<ProblemForReadModel>>(result);
            Assert.AreEqual(problemList.Count, result.Count);
        }

        [Test]
        public async Task GetProblemAsync_ShouldReturnProblemForRead_WithValidId()
        {
            // Arrange
            Guid guid = Guid.NewGuid();

            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemAsync(guid))
                    .Returns(Task.FromResult(new Problem() { Id = guid }));

            var problemService = new ProblemService(fakeProblemRepository.Object,
                null, null, null, _mapper, null, null);

            // Act
            var result = await problemService.GetProblemAsync(guid);

            // Assert
            Assert.IsInstanceOf<ProblemForReadModel>(result);
            Assert.AreEqual(guid, result.Id);
        }

        [Test]
        public void GetProblemAsync_ShouldThrowEntityNotFoundException_WithInvalidId()
        {
            // Arrange
            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                    .Returns(Task.FromResult<Problem>(null));

            var problemService = new ProblemService(fakeProblemRepository.Object,
                null, null, null, _mapper, _loggerStub, null);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => 
                await problemService.GetProblemAsync(Guid.NewGuid()));
        }

        [Test]
        public async Task CreateProblemAsync_ShouldReturnId_WithValidIds()
        {
            // Arrange
            Guid guid = Guid.NewGuid();

            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository.Setup(r => r.CreateProblemAsync(It.IsAny<Problem>()))
                .Callback((Problem problem) => { problem.Id = guid; });

            var bicycleServiceStub = new Mock<IBicycleService>();
            bicycleServiceStub.Setup(b => b.GetBicycleAsync(It.IsAny<Guid>()));

            var clientServiceStub = new Mock<IClientService>();
            clientServiceStub.Setup(c => c.GetClientAsync(It.IsAny<Guid>()));

            var partServiceStub = new Mock<IPartService>();
            partServiceStub.Setup(c => c.GetPartAsync(It.IsAny<Guid>()));

            var problemService = new ProblemService(fakeProblemRepository.Object,
                null, clientServiceStub.Object, bicycleServiceStub.Object, _mapper, 
                _loggerStub, partServiceStub.Object);
            
            // Act
            var result = await problemService.CreateProblemAsync(new ProblemForCreateModel());

            // Assert
            Assert.AreEqual(guid, result);
        }

        [Test]
        public void CreateProblemAsync_ShouldThrowEntityNotFoundException_WithInvalidIds()
        {
            // Arrange
            var fakeProblemRepository = new Mock<IProblemRepository>();

            var bicycleServiceStub = new Mock<IBicycleService>();
            bicycleServiceStub
                .Setup(b => b.GetBicycleAsync(It.IsAny<Guid>()))
                .Throws(new EntityNotFoundException());

            var clientServiceStub = new Mock<IClientService>();
            clientServiceStub
                .Setup(c => c.GetClientAsync(It.IsAny<Guid>()))
                .Throws(new EntityNotFoundException());

            var partServiceStub = new Mock<IPartService>();
            partServiceStub
                .Setup(c => c.GetPartAsync(It.IsAny<Guid>()))
                .Throws(new EntityNotFoundException());

            var problemService = new ProblemService(fakeProblemRepository.Object,
                null, clientServiceStub.Object, bicycleServiceStub.Object, _mapper,
                _loggerStub, partServiceStub.Object);

            // Act & Asset
            Assert.ThrowsAsync<EntityNotFoundException>(async () => 
                await problemService.CreateProblemAsync(new ProblemForCreateModel()));
        }

        [Test]
        public void DeleteProblemAsync_ShouldNotThrowException_WithValidId()
        {
            // Arrange
            Guid guid = Guid.NewGuid();
            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemAsync(guid))
                    .Returns(Task.FromResult(new Problem() { Id = guid }));
            fakeProblemRepository
                .Setup(r => r.DeleteProblemAsync(It.IsAny<Problem>()))
                .Verifiable();

            var problemService = new ProblemService(fakeProblemRepository.Object,
                null, null, null, _mapper, _loggerStub, null);

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await problemService.DeleteProblemAsync(guid));
            Assert.DoesNotThrow(() => fakeProblemRepository.Verify());
        }

        [Test]
        public void DeleteProblemAsync_ShouldThrowEntityNotFoundException_WithInvalidId()
        {
            // Arrange
            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                    .Returns(Task.FromResult<Problem>(null));
            fakeProblemRepository
                .Setup(r => r.DeleteProblemAsync(It.IsAny<Problem>()))
                .Verifiable();

            var problemService = new ProblemService(fakeProblemRepository.Object,
                null, null, null, _mapper, _loggerStub, null);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () => 
                await problemService.DeleteProblemAsync(Guid.NewGuid()));
            Assert.Throws<MockException>(() => fakeProblemRepository.Verify());
        }

        [Test]
        public void UpdateProblemAsync_ShouldUpdateProblem_WithValidIds()
        {
            // Arrange
            Guid guid = Guid.NewGuid();


            var fakeProblem = new Problem()
            {
                Id = guid,
                BicycleId = Guid.NewGuid(),
                ClientId = Guid.NewGuid(),
                Description = "Description",
                Place = "Place",
                ReceivingDate = new DateTime(2021, 7, 12)
            };

            var fakeUpdateProblem = new ProblemForUpdateModel()
            {
                BicycleId = Guid.NewGuid(),
                Description = "Description 2",
                Place = "Place 2"
            };

            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemAsync(guid))
                    .Returns(Task.FromResult(fakeProblem));
            fakeProblemRepository
                .Setup(r => r.UpdateProblemAsync(It.IsAny<Problem>()));

            var bicycleServiceStub = new Mock<IBicycleService>();
            bicycleServiceStub.Setup(b => b.GetBicycleAsync(It.IsAny<Guid>()));

            var problemService = new ProblemService(fakeProblemRepository.Object,
                null, null, bicycleServiceStub.Object, _mapper, _loggerStub, null);

            // Act
            var result = problemService.UpdateProblemAsync(guid, fakeUpdateProblem);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(fakeUpdateProblem.BicycleId, fakeProblem.BicycleId);
                Assert.AreEqual(fakeUpdateProblem.Description, fakeProblem.Description);
                Assert.AreEqual(fakeUpdateProblem.Place, fakeProblem.Place);
            });
        }

        [Test]
        public void UpdateProblemAsync_ShouldThrowEntityNotFoundException_WithInvalidProblemId()
        {
            // Arrange
            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                    .Returns(Task.FromResult<Problem>(null));
            fakeProblemRepository
                .Setup(r => r.UpdateProblemAsync(It.IsAny<Problem>()))
                .Verifiable();

            var bicycleServiceStub = new Mock<IBicycleService>();
            bicycleServiceStub.Setup(b => b.GetBicycleAsync(It.IsAny<Guid>()));

            var problemService = new ProblemService(fakeProblemRepository.Object,
                null, null, bicycleServiceStub.Object, _mapper, _loggerStub, null);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await problemService.UpdateProblemAsync(Guid.NewGuid(), new ProblemForUpdateModel()));
            Assert.Throws<MockException>(() => fakeProblemRepository.Verify());
        }

        [Test]
        public async Task GetPartListForProblemAsync_ShouldReturnPartList_NoData()
        {
            // Arrange
            var partDetailsList = GetPartDetailsList(hasData: false);

            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                    .Returns(Task.FromResult(new Problem()));

            var fakePartDetailRepository = new Mock<IPartDetailsRepository>();
            fakePartDetailRepository
                .Setup(r => r.GetPartDetailListAsync(It.IsAny<Guid>()))
                    .Returns(Task.FromResult(partDetailsList));

            var problemService = new ProblemService(fakeProblemRepository.Object,
               fakePartDetailRepository.Object, null, null, _mapper, _loggerStub, null);

            // Act
            var result = await problemService.GetPartListForProblemAsync(Guid.NewGuid());

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<List<PartDetailsForReadModel>>(result);
            Assert.AreEqual(partDetailsList.Count, result.Count);
        }

        [Test]
        public async Task GetPartListForProblemAsync_ShouldReturnPartList_WithData()
        {
            // Arrange
            var partDetailsList = GetPartDetailsList(hasData: true);

            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                    .Returns(Task.FromResult(new Problem()));

            var fakePartDetailRepository = new Mock<IPartDetailsRepository>();
            fakePartDetailRepository
                .Setup(r => r.GetPartDetailListAsync(It.IsAny<Guid>()))
                    .Returns(Task.FromResult(partDetailsList));

            var problemService = new ProblemService(fakeProblemRepository.Object,
               fakePartDetailRepository.Object, null, null, _mapper, _loggerStub, null);

            // Act
            var result = await problemService.GetPartListForProblemAsync(Guid.NewGuid());

            // Assert
            Assert.IsInstanceOf<List<PartDetailsForReadModel>>(result);
            Assert.AreEqual(partDetailsList.Count, result.Count);
        }

        [Test]
        public async Task GetPartForProblemAsync_ShouldReturnPart_WithValidId()
        {
            // Arrange
            var partDetail = new PartDetails()
            {
                Id = Guid.NewGuid(),
                Part = new Part()
                {
                    Id = Guid.NewGuid(),
                },
                ProblemId = Guid.NewGuid(),
                Amount = 3
            };

            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                    .Returns(Task.FromResult(new Problem()));

            var fakePartDetailsRepository = new Mock<IPartDetailsRepository>();
            fakePartDetailsRepository
                .Setup(r => r.GetPartDetailAsync(partDetail.ProblemId, partDetail.PartId))
                    .Returns(Task.FromResult(partDetail));

            var problemService = new ProblemService(fakeProblemRepository.Object,
               fakePartDetailsRepository.Object, null, null, _mapper, _loggerStub, null);

            // Act
            var result = await problemService.GetPartForProblemAsync(partDetail.ProblemId, partDetail.PartId);

            // Assert
            Assert.IsInstanceOf<PartDetailsForReadModel>(result);
            Assert.Multiple(() =>
            {
                Assert.AreEqual(partDetail.Part.Id, result.Part.Id);
                Assert.AreEqual(partDetail.Amount, result.Amount);
            });
        }

        [Test]
        public void GetPartForProblemAsync_ShouldThrowEntityNotFoundException_WithInvalidId()
        {
            // Arrange
            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                    .Returns(Task.FromResult(new Problem()));

            var fakePartDetailsRepository = new Mock<IPartDetailsRepository>();
            fakePartDetailsRepository
                .Setup(r => r.GetPartDetailAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .Returns(Task.FromResult<PartDetails>(null));

            var problemService = new ProblemService(fakeProblemRepository.Object,
               fakePartDetailsRepository.Object, null, null, _mapper, _loggerStub, null);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await problemService.GetPartForProblemAsync(Guid.NewGuid(), Guid.NewGuid()));
        }

        [Test]
        public async Task CreatePartForProblemAsync_ShouldReturnId_WithValidIds()
        {
            // Arrange
            Guid guid = Guid.NewGuid();

            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                    .Returns(Task.FromResult(new Problem()));

            var fakePartDetailsRepository = new Mock<IPartDetailsRepository>();
            fakePartDetailsRepository
                .Setup(r => r.CreatePartDetailAsync(It.IsAny<PartDetails>()))
                    .Callback((PartDetails partDetails) => partDetails.Id = guid);

            var problemService = new ProblemService(fakeProblemRepository.Object,
               fakePartDetailsRepository.Object, null, null, _mapper, _loggerStub, null);

            // Act
            var result = await problemService.CreatePartForProblemAsync(Guid.NewGuid(), new PartDetailsForCreateModel());

            // Assert
            Assert.AreEqual(guid, result);
        }

        [Test]
        public void CreatePartForProblemAsync_ShouldThrowArgumentexception_WhenSamePartsAlreadyAdded()
        {
            // Arrange
            Guid guid = Guid.NewGuid();

            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                    .Returns(Task.FromResult(new Problem()
                    {
                        PartDetails = new List<PartDetails>
                        {
                            new PartDetails
                            {
                                Part = new Part
                                {
                                    Id = guid
                                }
                            }
                        }
                    }));

            var fakePartDetailsRepository = new Mock<IPartDetailsRepository>();
            fakePartDetailsRepository
                .Setup(r => r.CreatePartDetailAsync(It.IsAny<PartDetails>()))
                .Verifiable();

            var fakePartDetail = new PartDetailsForCreateModel
            {
                PartId = guid
            };

            var problemService = new ProblemService(fakeProblemRepository.Object,
               fakePartDetailsRepository.Object, null, null, _mapper, _loggerStub, null);

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () =>
                await problemService.CreatePartForProblemAsync(Guid.NewGuid(), fakePartDetail));

            Assert.Throws<MockException>(() => fakePartDetailsRepository.Verify());
        }

        [Test]
        public void CreatePartForProblemAsync_ShouldThrowEntityNotFoundException_WithInvalidId()
        {
            // Arrange
            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                    .Returns(Task.FromResult<Problem>(null));

            var fakePartDetailsRepository = new Mock<IPartDetailsRepository>();
            fakePartDetailsRepository
                .Setup(r => r.CreatePartDetailAsync(It.IsAny<PartDetails>()))
                .Verifiable();

            var problemService = new ProblemService(fakeProblemRepository.Object,
               fakePartDetailsRepository.Object, null, null, _mapper, _loggerStub, null);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await problemService.CreatePartForProblemAsync(Guid.NewGuid(), new PartDetailsForCreateModel()));

            Assert.Throws<MockException>(() => fakePartDetailsRepository.Verify());
        }

        [Test]
        public void DeletePartForProblemAsync_ShouldNotThrowException_WithValidIds()
        {
            // Arrange
            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                    .Returns(Task.FromResult(new Problem()));

            var fakePartDetailsRepository = new Mock<IPartDetailsRepository>();
            fakePartDetailsRepository
                .Setup(r => r.GetPartDetailAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .Returns(Task.FromResult(new PartDetails()));

            fakePartDetailsRepository
                .Setup(r => r.DeletePartDetailAsync(It.IsAny<PartDetails>()))
                .Verifiable();

            var problemService = new ProblemService(fakeProblemRepository.Object,
               fakePartDetailsRepository.Object, null, null, _mapper, _loggerStub, null);

            // Act & Assert
            Assert.DoesNotThrowAsync(async () =>
                await problemService.DeletePartForProblemAsync(Guid.NewGuid(), Guid.NewGuid()));
            Assert.DoesNotThrow(() => fakePartDetailsRepository.Verify());
        }

        [Test]
        public void DeletePartForProblemAsync_ShouldThrowEntityNotFoundException_WithInvalidProblemId()
        {
            // Arrange
            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                    .Returns(Task.FromResult<Problem>(null));

            var fakePartDetailsRepository = new Mock<IPartDetailsRepository>();
            fakePartDetailsRepository
                .Setup(r => r.DeletePartDetailAsync(It.IsAny<PartDetails>()))
                .Verifiable();

            var problemService = new ProblemService(fakeProblemRepository.Object,
               fakePartDetailsRepository.Object, null, null, _mapper, _loggerStub, null);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await problemService.DeletePartForProblemAsync(Guid.NewGuid(), Guid.NewGuid()));
            Assert.Throws<MockException>(() => fakePartDetailsRepository.Verify());
        }

        [Test]
        public void DeletePartForProblemAsync_ShouldThrowEntityNotFoundException_WithInvalidPartId()
        {
            // Arrange
            var fakeProblemRepository = new Mock<IProblemRepository>();
            fakeProblemRepository
                .Setup(r => r.GetProblemAsync(It.IsAny<Guid>()))
                    .Returns(Task.FromResult(new Problem()));

            var fakePartDetailsRepository = new Mock<IPartDetailsRepository>();
            fakePartDetailsRepository
                .Setup(r => r.GetPartDetailAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                    .Returns(Task.FromResult<PartDetails>(null));

            fakePartDetailsRepository
                .Setup(r => r.DeletePartDetailAsync(It.IsAny<PartDetails>()))
                .Verifiable();

            var problemService = new ProblemService(fakeProblemRepository.Object,
               fakePartDetailsRepository.Object, null, null, _mapper, _loggerStub, null);

            // Act & Assert
            Assert.ThrowsAsync<EntityNotFoundException>(async () =>
                await problemService.DeletePartForProblemAsync(Guid.NewGuid(), Guid.NewGuid()));
            Assert.Throws<MockException>(() => fakePartDetailsRepository.Verify());
        }
    }
}
