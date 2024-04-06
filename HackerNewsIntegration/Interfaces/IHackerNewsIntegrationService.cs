using Core.Models;

namespace HackerNewsIntegration.Interfaces
{
    public interface IHackerNewsIntegrationService
    {
        Task<IEnumerable<HackerNewsStory>> GetStoriesAsync();
    }
}