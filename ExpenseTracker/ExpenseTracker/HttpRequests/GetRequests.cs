namespace ExpenseTracker.Web.HttpRequests
{
    public class GetRequests
    {
        private readonly HttpClient _httpClient;

        public GetRequests(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Generic GET method
        public async Task<T> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<T>();
            return result!;
        }

    }
}
