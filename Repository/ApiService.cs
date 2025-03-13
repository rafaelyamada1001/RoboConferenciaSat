using Aplication.Interfaces;
using System.Text;

namespace Infra
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> PostDataAsync(string endpoint, string jsonContent)
        {
            try
            {
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadAsStringAsync();

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
