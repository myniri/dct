using AutoFixture;
using BLL.Services;
using Core.Models;
using HackerNewsIntegration.Interfaces;
using HackerNewsIntegration.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace DCT_Tests
{
    public class HackerNewsService_Test
    {
        private readonly Mock<IHackerNewsIntegrationService> _mockHackerNewsStoryService = new Mock<IHackerNewsIntegrationService>();
        private readonly Mock<IDistributedCache> _mockCache = new Mock<IDistributedCache>();
        private readonly Mock<ILogger<HackerNewsService>> _mockLogger = new Mock<ILogger<HackerNewsService>>();
        private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();
        private readonly Fixture _fixture = new Fixture();
        private HackerNewsService _service;

        public HackerNewsService_Test()
        {
            var config = _fixture.Build<HackerNewsIntegrationOptions>()
                .With(c => c.CacheResponseForSeconds, 300)
                .Create();

            var options = Options.Create<HackerNewsIntegrationOptions>(config);
            this._service = new HackerNewsService(this._mockHackerNewsStoryService.Object, this._mockCache.Object,
                this._mockLogger.Object, options, this._mockMapper.Object);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(7)]
        [InlineData(152)]
        public async Task GetStoriesAsync_ExactRequestedNumber3(int expectedCount)
        {
            // Arrange
            var serviceResults = new List<HackerNewsStory>()
            {
                new HackerNewsStory(),
                new HackerNewsStory(),
                new HackerNewsStory(),
                new HackerNewsStory(),
                new HackerNewsStory(),
                new HackerNewsStory(),
                new HackerNewsStory(),
                new HackerNewsStory(),
                new HackerNewsStory(),
                new HackerNewsStory(),
                new HackerNewsStory(),
            };

            this._mockHackerNewsStoryService.Setup(x => x.GetStoriesAsync()).ReturnsAsync(serviceResults);

            // Act
            var results = await this._service.GetStoriesAsync(expectedCount);

            // Assert
            Assert.True(results.Count() <= expectedCount);
        }
    }
}