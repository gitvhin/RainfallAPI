using NUnit.Framework;
using Moq;
using AutoMapper;
using System.Threading.Tasks;
using RainfallAPI.Application.Contracts;
using RainfallAPI.Application.Exceptions;
using RainfallAPI.Application.Response;
using RainfallAPI.Application.Services;
using RainfallAPI.Constants;
using System.Collections.Generic;
using RainfallAPI.Application.Mapping;

namespace RainfallAPI.Tests
{
    public class RainfallServiceTests
    {
        [Test]
        public async Task GetRainfallReadingsAsync_CorrectStationId_ReturnsValidResponse()
        {
            // Arrange
            var externalApiServiceMock = new Mock<IExternalAPIService>();
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));

            var service = new RainfallService(externalApiServiceMock.Object, mapper);

            var externalApiResponse = new ExternalAPIResponse
            {
                Items = new List<Item>
                {
                    new Item { Id = "1", DateTime = DateTime.Now, Measure = "test", Value = 1.0 }                   
                }
            };

            externalApiServiceMock.Setup(x => x.GetRainfallReadingsFromExternalApiAsync(It.IsAny<string>(), It.IsAny<int>()))
                                  .ReturnsAsync(externalApiResponse);

            // Act
            var result = await service.GetRainfallReadingsAsync("CorrectStationId");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Readings);
        }

        [Test]
        public void GetRainfallReadingsAsync_WrongStationId_ThrowsNotFoundException()
        {
            // Arrange
            var externalApiServiceMock = new Mock<IExternalAPIService>();
            var mapperMock = new Mock<IMapper>();

            var service = new RainfallService(externalApiServiceMock.Object, mapperMock.Object);

            externalApiServiceMock.Setup(x => x.GetRainfallReadingsFromExternalApiAsync(It.IsAny<string>(), It.IsAny<int>()))
                                  .ThrowsAsync(new NotFoundException("stationId", Constants.ErrorMessages.NotFound));

            // Act & Assert
            Assert.ThrowsAsync<NotFoundException>(() => service.GetRainfallReadingsAsync("WrongStationId"));
        }

        [Test]        
        public void GetRainfallReadingsAsync_NoStationId_ThrowsInvalidRequestException()
        {
            // Arrange
            var externalApiServiceMock = new Mock<IExternalAPIService>();
            var mapperMock = new Mock<IMapper>();

            var service = new RainfallService(externalApiServiceMock.Object, mapperMock.Object);

            // Act & Assert
            Assert.ThrowsAsync<InvalidRequestException>(() => service.GetRainfallReadingsAsync(""));
        }
    }
}
