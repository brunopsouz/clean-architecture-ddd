using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;
using Xunit;

namespace WebApi.Test.Recipe.Register
{
    public class RegisterRecipeTest : RecipeBookClassFixture
    {
        private const string METHOD = "recipe";
        private readonly Guid _userIdentifier;

        public RegisterRecipeTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _userIdentifier = factory.GetUserIdentifier();
        }

        [Fact]
        public async Task Success()
        {
            // Cria uma nova receita com os dados de teste
            var request = RequestRecipeJsonBuilder.Build();

            // Cria o token JWT para autenticação do usuárioTeste
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            // Envia a requisição POST para o endpoint "Recipe" com o corpo da requisição como JSON
            var response = await DoPost(method: METHOD, request: request, token: token);

            // Verifica se o status da resposta é 201 Created
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            // Verifica se o conteúdo da resposta é do tipo application/json
            await using var responseStream = await response.Content.ReadAsStreamAsync();
            var responseData = await JsonDocument.ParseAsync(responseStream);

            // Verifica se a propriedade "title" do JSON retornado é igual ao título esperado
            responseData.RootElement.GetProperty("title").GetString().ShouldBe(request.Title);
            responseData.RootElement.GetProperty("id").GetString().ShouldNotBeNullOrEmpty();
        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Title_Empty(string culture)
        {
            // Cria uma nova receita com o título vazio
            var request = RequestRecipeJsonBuilder.Build();
            request.Title = string.Empty;

            // Cria o token JWT para autenticação do usuárioTeste
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);
            
            // Envia a requisição POST para o endpoint "Recipe" com o corpo da requisição como JSON
            var response = await DoPost(method: METHOD, request: request, token: token, culture: culture);

            // Verifica se o status da resposta é 400 Bad Request
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            // Verifica se o conteúdo da resposta é do tipo application/json
            await using var responseStream = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseStream);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            // Verifica se a mensagem de erro contém o texto esperado para o nome vazio.
            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("RECIPE_TITLE_EMPTY", new CultureInfo(culture));

            errors.ShouldSatisfyAllConditions(
                name => name.ShouldHaveSingleItem(),
                name => name.ShouldContain(error => error.GetString()!.Equals(expectedMessage))
                );
        }
    }
}
