using CommonTestUtilities.Tokens;
using Shouldly;
using Xunit;

namespace WebApi.Test.Dashboard
{
    public class GetDashboardInvalidTokenTest : RecipeBookClassFixture
    {
        private const string METHOD = "dashboard";

        public GetDashboardInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Error_Invalid_Token()
        {
            var response = await DoGet(METHOD, token: "token");
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Without_Token()
        {
            var response = await DoGet(METHOD, token: string.Empty);
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Token_With_User_NotFound()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
            var response = await DoGet(METHOD, token: token);
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}
