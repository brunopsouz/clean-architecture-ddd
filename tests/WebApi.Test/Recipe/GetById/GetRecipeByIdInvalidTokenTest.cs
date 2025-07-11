using CommonTestUtilities.idEncryption;
using CommonTestUtilities.Tokens;
using Shouldly;
using Xunit;

namespace WebApi.Test.Recipe.GetById
{
    public class GetRecipeByIdInvalidTokenTest : RecipeBookClassFixture
    {
        private readonly string METHOD = "recipe";

        public GetRecipeByIdInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Error_Token_Invalid()
        {
            var id = IdEncripterBuilder.Build().Encode(1);

            var response = await DoGet($"{METHOD}/{id}", "invalid_token");

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Token_Empty()
        {
            var id = IdEncripterBuilder.Build().Encode(1);
            var response = await DoGet($"{METHOD}/{id}", string.Empty);
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Token_With_User_NotFound()
        {
            var id = IdEncripterBuilder.Build().Encode(1);

            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

            var response = await DoGet($"{METHOD}/{id}", token: token);

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);

        }
    }
}
