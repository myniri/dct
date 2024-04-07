namespace HttpClientHelper.Interfaces
{
    public interface IBaseHttpClient
    {
        Task<TResponse> GetAsync<TResponse>(string uri);
    }
}