using CommonTestUtilities.Requests;
using Microsoft.AspNetCore.Mvc.Testing;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;
using Xunit;

namespace WebApi.Test.User.Register
{
    /// <summary>
    /// WebApplicationFactory cria um servidor de teste para a aplicação. Tenho que chamar a classe "Program" pois ela que executa a aplicação.
    /// Nesse momento é necessário criar um servidor de teste para a aplicação, pois o teste de integração não é unitário.
    /// Teste de integração executa a API e faz de fato requisições na API testando a resposta da API.
    /// 
    /// Obs: Criar um construtor para Program.cs, pois o WebApplicationFactory não consegue instanciar a classe Program.cs.
    /// </summary>
    public class RegisterUserTest : RecipeBookClassFixture
    {
        private readonly string method = "user";

        public RegisterUserTest(CustomWebApplicationFactory factory) : base(factory) { }

        [Fact]
        public async Task Success()
        {
            // Cria um novo usuário com os dados de teste
            var request = RequestRegisterUserJsonBuilder.Build();

            //  Envia a requisição POST para o endpoint "User" com o corpo da requisição como JSON
            var response = await DoPost(method, request);

            // Verifica se o status da resposta é 201 Created
            response.StatusCode.ShouldBe(HttpStatusCode.Created);

            // Verifica se o conteúdo da resposta é do tipo application/json
            await using var responseStream = await response.Content.ReadAsStreamAsync();

            // Verifica se o conteúdo da resposta é do tipo application/json
            var responseData = await JsonDocument.ParseAsync(responseStream);

            ////Pega a propriedade "name" do JSON retornado como string. 
            //var name = responseData.RootElement.GetProperty("name").GetString();

            //// Verifica se não é nulo nem whitespace
            //name.ShouldNotBeNullOrWhiteSpace();

            //// Verifica se é igual ao nome esperado
            //name.ShouldBe(request.Name);

            responseData.RootElement.GetProperty("name").GetString().ShouldSatisfyAllConditions(
                name => name.ShouldNotBeNullOrWhiteSpace(),
                name => name.ShouldBe(request.Name)
                );

            responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString()
                .ShouldNotBeNullOrWhiteSpace();

        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Empty_Name(string culture)
        {
            var request = RequestRegisterUserJsonBuilder.Build();
            request.Name = string.Empty;

            var response = await DoPost(method, request, culture);

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

            errors.ShouldHaveSingleItem();
            errors.ShouldContain(error => error.GetString()!.Equals(expectedMessage));
        }
    }
}
