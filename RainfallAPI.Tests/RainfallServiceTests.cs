using System.Net;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using RainfallAPI.Application.Contracts;
using RainfallAPI.Application.Mappers;
using RainfallAPI.Application.Models;
using RainfallAPI.Application.Services;
using RainfallAPI.Constants;

namespace RainfallAPI.Tests
{
    /// <summary>
    /// Contains unit tests for the <see cref="RainfallService"/> class.
    /// </summary>
    public class RainfallServiceTests
    {
        private Mock<IConfiguration> _configurationMock;
        private Mock<IConfigurationSection> _configurationSectionMock;
        private Mock<IHttpClientWrapper> _httpClientWrapperMock;
        private Mock<ILogger<RainfallService>> _loggerMock;

        private IMapper _mapper;
        private IRainfallService _rainfallService;

        /// <summary>
        /// Sets up the necessary mocks and dependencies for the tests.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _configurationMock = new Mock<IConfiguration>();
            _configurationSectionMock = new Mock<IConfigurationSection>();

            _configurationSectionMock
               .Setup(x => x.Value)
               .Returns("https://environment.data.gov.uk");

            _configurationMock
               .Setup(x => x.GetSection("ApiSettings:BaseUrl"))
               .Returns(_configurationSectionMock.Object);

            _httpClientWrapperMock = new Mock<IHttpClientWrapper>();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()));
            _loggerMock = new Mock<ILogger<RainfallService>>();
            _rainfallService = new RainfallService(_httpClientWrapperMock.Object, _configurationMock.Object, _mapper, _loggerMock.Object);
        }

        /// <summary>
        /// Tests the <see cref="RainfallService.GetRainfallReadingsAsync(string, int)"/> method when correct station ID is provided.
        /// </summary>
        [Test]
        public async Task GetRainfallReadingsAsync_CorrectStationId_ReturnsRainfallReadingResponse()
        {
            // Arrange
            var stationId = "1";
            var count = 10;

            var expectedResponse = new ExternalAPIResponse
            {
                Items = new List<Item> {
                    new Item { Id = "1", DateTime = DateTime.Now, Measure = "test1", Value = 1.0 },
                    new Item { Id = "2", DateTime = DateTime.Now, Measure = "test2", Value = 2.0 }
                }
            };

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponse))
            };

            _httpClientWrapperMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(httpResponse);

            // Act
            var result = await _rainfallService.GetRainfallReadingsAsync(stationId, count);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Readings);
            Assert.AreEqual(expectedResponse.Items.Count, result.Readings.Count);

            for (int i = 0; i < expectedResponse.Items.Count; i++)
            {
                Assert.AreEqual(expectedResponse.Items[i].DateTime, result.Readings[i].DateMeasured);
                Assert.AreEqual((decimal)expectedResponse.Items[i].Value, result.Readings[i].AmountMeasured);
            }
        }

        /// <summary>
        /// Tests the <see cref="RainfallService.GetRainfallReadingsAsync(string, int)"/> method when an invalid station ID is provided.
        /// </summary>
        [Test]
        public void GetRainfallReadingsAsync_InvalidStationId_ThrowsArgumentException()
        {
            // Arrange
            var stationId = "";
            var count = 10;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _rainfallService.GetRainfallReadingsAsync(stationId, count));
        }

        /// <summary>
        /// Tests the <see cref="RainfallService.GetRainfallReadingsAsync(string, int)"/> method when no station ID is provided.
        /// </summary>
        [Test]
        public void GetRainfallReadingsAsync_NoStationId_ThrowsArgumentNullException()
        {
            // Arrange
            string stationId = " ";
            var count = 10;

            // Act & Assert
            Assert.ThrowsAsync<ArgumentException>(async () => await _rainfallService.GetRainfallReadingsAsync(stationId, count));
        }

        /// <summary>
        /// Tests the <see cref="RainfallService.GetRainfallReadingsAsync(string, int)"/> method when an empty response is received.
        /// </summary>
        [Test]
        public async Task GetRainfallReadingsAsync_EmptyResponse_ReturnsEmptyReadings()
        {
            // Arrange
            var stationId = "1";
            var count = 10;

            var expectedResponse = new ExternalAPIResponse
            {
                Items = new List<Item>() // Empty response
            };

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponse))
            };

            _httpClientWrapperMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(httpResponse);

            // Act & Assert
            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () => await _rainfallService.GetRainfallReadingsAsync(stationId, count));
            Assert.AreEqual(ErrorMessages.NotFound, exception.Message);
        }

        /// <summary>
        /// Tests the logging behavior when the <see cref="RainfallService.GetRainfallReadingsAsync(string, int)"/> method returns a successful response.
        /// </summary>
        [Test]
        public async Task GetRainfallReadingsAsync_LogOKResponse()
        {
            // Arrange
            var stationId = "1";
            var count = 10;

            var expectedResponse = new ExternalAPIResponse
            {
                Items = new List<Item> {
                    new Item { Id = "1", DateTime = DateTime.Now, Measure = "test1", Value = 1.0 },
                    new Item { Id = "2", DateTime = DateTime.Now, Measure = "test2", Value = 2.0 }
                }
            };

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponse))
            };

            _httpClientWrapperMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(httpResponse);

            var rainfallService = new RainfallService(_httpClientWrapperMock.Object, _configurationMock.Object, _mapper, _loggerMock.Object);

            // Act
            var result = await rainfallService.GetRainfallReadingsAsync(stationId, count);

            // Assert

            // Verify logging calls
            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        /// <summary>
        /// Tests the logging behavior when the <see cref="RainfallService.GetRainfallReadingsAsync(string, int)"/> method throws an <see cref="ArgumentException"/>.
        /// </summary>
        [Test]
        public void GetRainfallReadingsAsync_InvalidStationId_LogsArgumentException()
        {
            // Arrange
            var stationId = "";
            var count = 10;

            // Act & Assert
            var exception = Assert.ThrowsAsync<ArgumentException>(async () => await _rainfallService.GetRainfallReadingsAsync(stationId, count));

            // Verify logging calls
            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }

        /// <summary>
        /// Tests the logging behavior when the <see cref="RainfallService.GetRainfallReadingsAsync(string, int)"/> method throws an <see cref="InvalidOperationException"/>.
        /// </summary>
        [Test]
        public void GetRainfallReadingsAsync_InvalidStationId_LogsInvalidOperationException()
        {
            // Arrange
            var stationId = "1";
            var count = 10;

            var expectedResponse = new ExternalAPIResponse
            {
                Items = new List<Item>()
            };

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(expectedResponse))
            };

            _httpClientWrapperMock.Setup(x => x.GetAsync(It.IsAny<string>())).ReturnsAsync(httpResponse);

            // Act & Assert
            var exception = Assert.ThrowsAsync<InvalidOperationException>(async () => await _rainfallService.GetRainfallReadingsAsync(stationId, count));

            // Verify logging calls
            _loggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
