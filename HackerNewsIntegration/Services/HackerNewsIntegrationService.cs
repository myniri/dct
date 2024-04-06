using Core.Models;
using HackerNewsIntegration.Interfaces;
using HackerNewsIntegration.Options;
using HttpClientHelper.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HackerNewsIntegration.Services
{
    public class HackerNewsIntegrationService : IHackerNewsIntegrationService
    {
        private readonly IBaseHttpClient _httpClient;
        private readonly ILogger<HackerNewsIntegrationService> _logger;
        private readonly HackerNewsIntegrationOptions _options;

        public HackerNewsIntegrationService(IBaseHttpClient httpClient,
            IOptions<HackerNewsIntegrationOptions> options, ILogger<HackerNewsIntegrationService> logger)
        {
            this._httpClient = httpClient;
            this._options = options.Value;
            this._logger = logger;
        }

        public async Task<IEnumerable<HackerNewsStory>> GetStoriesAsync()
        {
            try
            {
                var stories = await FetchAllStoriesAsync();
                return stories;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Exception in GetStoriesAsync");
                throw;
            }
        }

        private async Task<List<HackerNewsStory>> FetchAllStoriesAsync()
        {
            var storyIds = await this._httpClient.GetAsync<IEnumerable<int>>($"{_options.BaseUrl}/{_options.BestStoriesEndpoint}");
            var tasks = storyIds.Select(storyId =>
            {
                var itemUrl = $"{this._options.BaseUrl}/{string.Format(this._options.ItemEndpoint, storyId)}";
                var result = this._httpClient.GetAsync<HackerNewsStory>(itemUrl);

                return result;
            });

            var stories = (await Task.WhenAll(tasks)).Where(s => s != null).ToList();

            return stories;
        }
    }
}