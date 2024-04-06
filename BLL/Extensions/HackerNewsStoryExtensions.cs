using BLL.DTOs;
using Core.Models;

namespace BLL.Extensions
{
    public static class HackerNewsStoryExtensions
    {
        public static IEnumerable<HackerNewsStoryDTO> AllToHackerNewsStoryDTO(this IEnumerable<HackerNewsStory> stories)
        {
            if (stories != null && stories.Any())
            {
                IEnumerable<HackerNewsStoryDTO> hackerNewsStoryDtos = stories.Select(s => s.ToHackerNewsStoryDTO()).ToList();
                return hackerNewsStoryDtos;
            }

            return null;
        }

        public static HackerNewsStoryDTO ToHackerNewsStoryDTO(this HackerNewsStory story)
        {
            HackerNewsStoryDTO hackerNewsStoryDto = new HackerNewsStoryDTO()
            {
                Score = (uint)story.Score,
                Uri = story.Url,
                PostedBy = story.By,
                CommentCount = (uint)(story.Kids == null || !story.Kids.Any() ? 0 : story.Kids.Count()),
                Time = DateTimeOffset.FromUnixTimeSeconds(story.Time).UtcDateTime,
                Title = story.Title,
            };

            return hackerNewsStoryDto;
        }
    }
}