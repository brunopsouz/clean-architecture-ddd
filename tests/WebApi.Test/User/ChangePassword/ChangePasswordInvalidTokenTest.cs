using CommonTestUtilities.Tokens;
using RecipeBook.Communication.Requests;
using Shouldly;
using System.Net;
using Xunit;

namespace WebApi.Test.User.ChangePassword
{
    public class ChangePasswordInvalidTokenTest : RecipeBookClassFixture
    {
        private const string METHOD = "user/change-password";

        public ChangePasswordInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Error_InvalidToken()
        {
            var request = new RequestChangePasswordJson();

            var response = await DoPut(METHOD, request, "invalid-token");

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        }

        [Fact]
        public async Task Error_EmptyToken()
        {
            var request = new RequestChangePasswordJson();
            var response = await DoPut(METHOD, request, token: string.Empty);
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Token_With_User_NotFound()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var request = new RequestChangePasswordJson();

            var response = await DoPut(METHOD, request, token);

            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        }
    }
}
