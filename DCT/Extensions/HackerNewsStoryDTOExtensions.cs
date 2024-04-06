using BLL.DTOs;
using DCT.ResponseModels;

namespace DCT.Extensions
{
    public static class HackerNewsStoryDTOExtensions
    {
        public static IEnumerable<HackerNewsBestStoryResponse> AllToHackerNewsBestStoryResponse(this IEnumerable<HackerNewsStoryDTO> stories)
        {
            if (stories != null && stories.Any())
            {
                IEnumerable<HackerNewsBestStoryResponse> hackerNewsBestStoryResponses = stories.Select(s => s.ToHackerNewsBestStoryResponse()).ToList();
                return hackerNewsBestStoryResponses;
            }

            return null;
        }

        public static HackerNewsBestStoryResponse ToHackerNewsBestStoryResponse(this HackerNewsStoryDTO story)
        {
            HackerNewsBestStoryResponse hackerNewsBestStoryResponse = new HackerNewsBestStoryResponse()
            {
                Score = (uint)story.Score,
                Uri = story.Uri,
                PostedBy = story.PostedBy,
                CommentCount = story.CommentCount,
                Time = story.Time,
                Title = story.Title,
            };

            return hackerNewsBestStoryResponse;
        }
    }
}