namespace TelegramBot.Infrastructure.Services
{
    public class HttpClientFactoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public HttpClientFactoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient CreateClient(string name)
        {
            return _httpClientFactory.CreateClient(name);
        }
    }
}
