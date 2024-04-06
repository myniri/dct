using HttpClientHelper.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace HttpClientHelper.Services
{
    public class BaseHttpClient : IBaseHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BaseHttpClient> _logger;

        public BaseHttpClient(HttpClient httpClient, ILogger<BaseHttpClient> logger)
        {
            this._httpClient = httpClient;
            this._logger = logger;
        }

        public async Task<TResponse> GetAsync<TResponse>(string uri)
        {
            try
            {
                var response = await this._httpClient.GetAsync(uri);

                if (!response.IsSuccessStatusCode)
                {
                    this._logger.LogWarning($"Request to {uri} returned {response.StatusCode}");
                    response.EnsureSuccessStatusCode();
                }

                var contentStream = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<TResponse>(contentStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                return result;
            }
            catch (HttpRequestException e)
            {
                this._logger.LogError(e, $"Exception in GetAsync by URI: {uri}");
                throw;
            }
        }
    }
}