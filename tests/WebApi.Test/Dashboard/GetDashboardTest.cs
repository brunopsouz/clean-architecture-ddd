using CommonTestUtilities.Tokens;
using Shouldly;
using System.Text.Json;
using Xunit;

namespace WebApi.Test.Dashboard
{
    public class GetDashboardTest : RecipeBookClassFixture
    {
        private const string METHOD = "dashboard";
        private readonly Guid _userIdentifier;

        public GetDashboardTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success() 
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoGet(METHOD, token: token);
            
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("recipes").GetArrayLength().ShouldBeGreaterThan(0);

        }
    }
}
