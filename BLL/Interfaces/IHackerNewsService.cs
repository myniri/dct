using BLL.DTOs;

namespace BLL.Interfaces
{
    public interface IHackerNewsService
    {
        Task<IEnumerable<HackerNewsStoryDTO>> GetStoriesAsync(int count);
    }
}