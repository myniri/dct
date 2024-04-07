using BLL.Interfaces;
using DCT.Extensions;
using DCT.ResponseModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DCT.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HackerNewsController : Controller
    {
        private readonly IHackerNewsService _hackerNewsService;
        private readonly ILogger<HackerNewsController> _logger;

        public HackerNewsController(ILogger<HackerNewsController> logger, IHackerNewsService hackerNewsService)
        {
            this._logger = logger;
            this._hackerNewsService = hackerNewsService;
        }

        [HttpGet("best-stories/{count}")]
        [ProducesResponseType(typeof(IEnumerable<HackerNewsBestStoryResponse>), 200)]
        [SwaggerOperation(
            Summary = "Get the best `N` Hacker News stories",
            Description = "Retrieves a list of best Hacker News stories.",
            OperationId = "GetBestStories")]
        public async Task<ActionResult<IEnumerable<HackerNewsBestStoryResponse>>> GetBestStories(int count)
        {
            try
            {
                if (count <= 0)
                {
                    this._logger.LogError($"The requested count value most be more than 0");
                    return this.BadRequest("The requested count value most be more than 0.");
                }

                var stories = await this._hackerNewsService.GetStoriesAsync(count);
                var response = stories.AllToHackerNewsBestStoryResponse();

                return this.Ok(response);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, $"Exception in GetBestStories");
                return this.BadRequest();
            }
        }
    }
}