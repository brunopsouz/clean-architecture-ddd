using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;
using Xunit;

namespace WebApi.Test.User.Profile
{
    public class GetUserProfileInvalidTokenTest : RecipeBookClassFixture
    {
        private readonly string METHOD = "user"; 

        public GetUserProfileInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Error_Invalid_Token()
        {
            var response = await DoGet(METHOD, token: "tokenInvalid");

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        }

        [Fact]
        public async Task Error_Empty_Token()
        {
            var response = await DoGet(METHOD, token: string.Empty);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Token_With_User_NotFound()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
            var response = await DoGet(METHOD, token: token);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }
    }
}
