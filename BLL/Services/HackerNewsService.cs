using BLL.DTOs;
using BLL.Extensions;
using BLL.Interfaces;
using Core.Models;
using HackerNewsIntegration.Interfaces;
using HackerNewsIntegration.Options;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace BLL.Services
{
    public class HackerNewsService: IHackerNewsService
    {
        private readonly ILogger<HackerNewsService> _logger;
        private readonly HackerNewsIntegrationOptions _options;
        private readonly IHackerNewsIntegrationService _hackerNewsStoryService;
        private readonly IDistributedCache _cache;
        private const string CachedStoriesKey = "stories";

        public HackerNewsService(IHackerNewsIntegrationService hackerNewsStoryService, IDistributedCache cache,
            ILogger<HackerNewsService> logger, IOptions<HackerNewsIntegrationOptions> options)
        {
            this._hackerNewsStoryService = hackerNewsStoryService;
            this._cache = cache;
            this._logger = logger;
            this._options = options.Value;
        }

        public async Task<IEnumerable<HackerNewsStoryDTO>> GetStoriesAsync(int count)
        {
            try
            {
                var cachedStories = await this.TryGetCachedStoriesAsync(count);
                if (cachedStories != null && cachedStories.Any())
                {
                    return cachedStories;
                }

                var stories = await this._hackerNewsStoryService.GetStoriesAsync();
                if (stories != null && stories.Any())
                {
                    await this.CacheStoriesAsync(stories.OrderByDescending(story => story.Score));

                    var fetchedStories = stories.Take(count).AllToHackerNewsStoryDTO();

                    return fetchedStories;
                }

                return null;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Exception in GetStoriesAsync");
                throw;
            }
        }

        private async Task<IEnumerable<HackerNewsStoryDTO>> TryGetCachedStoriesAsync(int count)
        {
            var cachedStories = await _cache.GetStringAsync(HackerNewsService.CachedStoriesKey);
            if (!string.IsNullOrEmpty(cachedStories))
            {
                var storiesFromCache = JsonSerializer.Deserialize<List<HackerNewsStory>>(cachedStories);
                if (storiesFromCache != null && storiesFromCache.Any())
                {
                    var result = storiesFromCache.Take(count).AllToHackerNewsStoryDTO().ToList();
                    return result;
                }
            }

            return null;
        }

        private async Task CacheStoriesAsync(IEnumerable<HackerNewsStory> stories)
        {
            await this._cache.SetStringAsync(HackerNewsService.CachedStoriesKey, JsonSerializer.Serialize(stories),
                new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(this._options.CacheResponseForSeconds) });
        }
    }
}