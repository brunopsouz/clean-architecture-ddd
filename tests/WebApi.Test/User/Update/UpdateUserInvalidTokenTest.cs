using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Text.Json;
using Xunit;

namespace WebApi.Test.User.Update
{
    public class UpdateUserInvalidTokenTest : RecipeBookClassFixture
    {

        private const string METHOD = "user";

        public UpdateUserInvalidTokenTest(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task Error_Invalid_Token()
        {
            // Criei dados de um usuário somente para teste.
            var request = RequestUpdateUserJsonBuilder.Build();

            // Faço a chamada com o método PUT, a request e um token inválido.
            var response = await DoPut(METHOD, request, "invalid_token");

            // Espero Unauthorized como resposta pois o token é inválido.
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);

        }

        [Fact]
        public async Task Error_Empty_Token()
        {
            // Criei dados de um usuário somente para teste.
            var request = RequestUpdateUserJsonBuilder.Build();

            // Faço a chamada com o método PUT, a request e um token vazio.
            var response = await DoPut(METHOD, request, token: string.Empty);

            // Espero Unauthorized como resposta pois o token é vazio.
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Error_Token_With_User_NotFound()
        {
            var request = RequestUpdateUserJsonBuilder.Build();

            var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid()); // Gerando um token para um usuário que não existe.

            var response = await DoPut(METHOD, request, token);

            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.Unauthorized);
        }
    }
}
