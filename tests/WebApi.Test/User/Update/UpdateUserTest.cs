using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Text.Json;
using WebApi.Test.InlineData;
using Xunit;

namespace WebApi.Test.User.Update
{
    public class UpdateUserTest : RecipeBookClassFixture
    {

        private const string METHOD = "user";
        private readonly Guid _userIdentifier;

        public UpdateUserTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            // Criei dados de um usuário somente para teste.
            var request = RequestUpdateUserJsonBuilder.Build();

            // cria o token JWT para autenticação do usuárioTeste.
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            // Faço a chamada com o método PUT, a request e o token JWT.
            var response = await DoPut(METHOD, request, token);

            // Espero NoContent como resposta pq meu controller está configurado para retornar NoContent.
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.NoContent);
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string culture)
        {
            var request = RequestUpdateUserJsonBuilder.Build();
            request.Name = string.Empty; // Deixando o nome vazio para gerar erro.

            // Cria o token JWT para autenticação do usuárioTeste.
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            // Faço a chamada com o método PUT, a request e o token JWT.
            var response = await DoPut(METHOD, request, token, culture);

            // Espero BadRequest como resposta pois o nome do user está vazio.
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);

            // Verifica se o conteúdo da resposta é do tipo application/json
            await using var responseBody = await response.Content.ReadAsStreamAsync();

            // Lê o conteúdo da resposta como um JsonDocument
            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            // Verifica se a mensagem de erro contém o texto esperado para o nome vazio.
            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            errors.ShouldSatisfyAllConditions(
                name => name.ShouldHaveSingleItem(),
                name => name.ShouldContain(error => error.GetString()!.Equals(expectedMessage))
                );

        }

    }
}
