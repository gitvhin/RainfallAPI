using NUnit.Framework;
using Moq;
using AutoMapper;
using System.Threading.Tasks;
using RainfallAPI.Application.Contracts;
using RainfallAPI.Application.Exceptions;
using RainfallAPI.Application.Response;
using RainfallAPI.Application.Services;
using RainfallAPI.Constants;
using System;
using System.Collections.Generic;
using RainfallAPI.Application.Mapping;

namespace RainfallAPI.Tests
{
    public class RainfallServiceTests
    {
        private Mock<IExternalAPIService> _externalApiServiceMock;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _externalApiServiceMock = new Mock<IExternalAPIService>();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));
        }

        [Test]
        public async Task GetRainfallReadingsAsync_CorrectStationId_ReturnsValidResponse()
        {
            // Arrange
            var expectedItems = new List<Item>
            {
                new Item { Id = "1", DateTime = DateTime.Now, Measure = "test", Value = 1.0 }
            };

            var externalApiResponse = new ExternalAPIResponse
            {
                Items = expectedItems
            };

            _externalApiServiceMock.Setup(x => x.GetRainfallReadingsFromExternalApiAsync("CorrectStationId", It.IsAny<int>()))
                                  .ReturnsAsync(externalApiResponse);

            var service = new RainfallService(_externalApiServiceMock.Object, _mapper);

            // Act
            var result = await service.GetRainfallReadingsAsync("CorrectStationId");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedItems.Count, result.Readings.Count);            
        }

        [Test]
        public void GetRainfallReadingsAsync_WrongStationId_ThrowsNotFoundException()
        {
            // Arrange
            _externalApiServiceMock.Setup(x => x.GetRainfallReadingsFromExternalApiAsync("WrongStationId", It.IsAny<int>()))
                                  .ThrowsAsync(new NotFoundException("stationId", Constants.ErrorMessages.NotFound));

            var service = new RainfallService(_externalApiServiceMock.Object, _mapper);

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => service.GetRainfallReadingsAsync("WrongStationId"));
        }

        [Test]
        public void GetRainfallReadingsAsync_NoStationId_ThrowsInvalidRequestException()
        {
            // Arrange
            var service = new RainfallService(_externalApiServiceMock.Object, _mapper);

            // Act & Assert
            Assert.ThrowsAsync<InvalidRequestException>(() => service.GetRainfallReadingsAsync(""));
        }
    }
}
