using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace WebApi.Test
{
    public class RecipeBookClassFixture : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public RecipeBookClassFixture(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        protected async Task<HttpResponseMessage> DoPost(string method, object request, string culture = "en")
        {
            ChangeRequestCulture(culture);

            return await _client.PostAsJsonAsync(method, request);
        }

        protected async Task<HttpResponseMessage> DoGet(string method, string token = "", string culture = "en")
        {
            ChangeRequestCulture(culture);
            AuthorizeRequest(token);

            return await _client.GetAsync(method);
        }

        protected async Task<HttpResponseMessage> DoPut(string method, object request, string token, string culture = "en")
        {
            ChangeRequestCulture(culture);
            AuthorizeRequest(token);
            return await _client.PutAsJsonAsync(method, request);
        }

        private void ChangeRequestCulture(string culture)
        {
            if (_client.DefaultRequestHeaders.Contains("Accept-Language"))
                _client.DefaultRequestHeaders.Remove("Accept-Language");

            _client.DefaultRequestHeaders.Add("Accept-Language", culture);
        }

        private void AuthorizeRequest(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return;
            
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
        }
    }
}
