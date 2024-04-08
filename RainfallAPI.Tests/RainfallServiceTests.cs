using NUnit.Framework;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using RainfallAPI.Application.Models;
using RainfallAPI.Application.Services;
using RainfallAPI.Domain.Entities;
using Newtonsoft.Json;
using RainfallAPI.Application.Mappers;

namespace RainfallAPI.Tests
{
    [TestFixture]
    public class RainfallServiceTests
    {
        private Mock<IHttpClientFactory> _mockHttpClientFactory;
        private Mock<ILogger<RainfallService>> _mockLogger;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _mockLogger = new Mock<ILogger<RainfallService>>();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));


        }

        [Test]
        public async Task GetRainfallReadingsAsync_CorrectStationId_ReturnsReadings()
        {
            // Arrange
            var expectedResponse = new ExternalAPIResponse
            {
                Items = new List<Item> {
                    new Item { Id = "1", DateTime = DateTime.Now, Measure = "test1", Value = 1.0 },
                    new Item { Id = "2", DateTime = DateTime.Now, Measure = "test2", Value = 2.0 }
                }
            };

            SetupHttpClient(HttpStatusCode.OK, expectedResponse);
            var service = new RainfallService(_mockHttpClientFactory.Object, _mapper, _mockLogger.Object);

            // Act
            var result = await service.GetRainfallReadingsAsync("correctStationId");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Readings);
            Assert.AreEqual(expectedResponse.Items.Count, result.Readings.Count);

            for (int i = 0; i < expectedResponse.Items.Count; i++)
            {
                Assert.AreEqual(expectedResponse.Items[i].DateTime, result.Readings[i].DateMeasured);
                Assert.AreEqual((decimal)expectedResponse.Items[i].Value, result.Readings[i].AmountMeasured);
            }

            // Verify logging calls
            _mockLogger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
        [Test]
        public async Task GetRainfallReadingsAsync_InvalidStationId_ThrowsHttpRequestException()
        {
            // Arrange
            var service = new RainfallService(_mockHttpClientFactory.Object, _mapper, _mockLogger.Object);

            // Act & Assert
            var exception = Assert.ThrowsAsync<HttpRequestException>(async () => await service.GetRainfallReadingsAsync(""));
            Assert.AreEqual(HttpStatusCode.BadRequest, exception.StatusCode);

            // Verify logging calls
            _mockLogger.Verify(
               x => x.Log(
                   It.IsAny<LogLevel>(),
                   It.IsAny<EventId>(),
                   It.IsAny<It.IsAnyType>(),
                   exception,
                   (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
               Times.Once);
        }
        [Test]
        public void GetRainfallReadingsAsync_NoStationId_ThrowsHttpRequestException()
        {
            // Arrange
            var service = new RainfallService(_mockHttpClientFactory.Object, _mapper, _mockLogger.Object);

            // Act & Assert
            var exception = Assert.ThrowsAsync<HttpRequestException>(async () => await service.GetRainfallReadingsAsync(" "));
            Assert.AreEqual(HttpStatusCode.BadRequest, exception.StatusCode);

            // Verify logging calls
            _mockLogger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    exception,
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
        [Test]
        public async Task GetRainfallReadingsAsync_NoReadingsFound_ThrowsHttpRequestException()
        {
            // Arrange
            var expectedResponse = new ExternalAPIResponse
            {
                Items = new List<Item> {}
            };

            SetupHttpClient(HttpStatusCode.OK, expectedResponse);

            var service = new RainfallService(_mockHttpClientFactory.Object, _mapper, _mockLogger.Object);

            // Act & Assert
            var exception = Assert.ThrowsAsync<HttpRequestException>(async () => await service.GetRainfallReadingsAsync("stationIdWithNoReadings"));
            Assert.AreEqual(HttpStatusCode.NotFound, exception.StatusCode);

            // Verify logging calls
            _mockLogger.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    exception,
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        private void SetupHttpClient(HttpStatusCode statusCode, ExternalAPIResponse content)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = statusCode,
                   Content = new StringContent(JsonConvert.SerializeObject(content))
               });

            var httpClient = new HttpClient(handlerMock.Object)
            {
                BaseAddress = new Uri("https://environment.data.gov.uk")
            };

            _mockHttpClientFactory.Setup(factory => factory.CreateClient(It.IsAny<string>())).Returns(httpClient);
        }
    }
}