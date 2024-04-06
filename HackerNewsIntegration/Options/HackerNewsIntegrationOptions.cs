namespace HackerNewsIntegration.Options
{
    public class HackerNewsIntegrationOptions
    {
        public string BaseUrl { get; set; }

        public string BestStoriesEndpoint { get; set; }

        public string ItemEndpoint { get; set; }

        public double CacheResponseForSeconds { get; set; }
    }
}