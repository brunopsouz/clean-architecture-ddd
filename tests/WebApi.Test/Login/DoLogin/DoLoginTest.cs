using CommonTestUtilities.Requests;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;
using Xunit;

namespace WebApi.Test.Login.DoLogin
{
    public class DoLoginTest : RecipeBookClassFixture
    {
        private readonly string method = "login";

        private readonly string _email;
        private readonly string _password;
        private readonly string _name;

        public DoLoginTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _email = factory.GetEmail();
            _password = factory.GetPassword();
            _name = factory.GetName();
        }

        [Fact]
        public async Task Success()
        {
            var request = new RequestLoginJson
            {
                Email = _email,
                Password = _password
            };

            //  Envia a requisição POST para o endpoint "User" com o corpo da requisição como JSON
            var response = await DoPost(method, request);

            // Verifica se o status da resposta é 201 Ok
            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            // Verifica se o conteúdo da resposta é do tipo application/json
            await using var responseStream = await response.Content.ReadAsStreamAsync();

            // Verifica se o conteúdo da resposta é do tipo application/json
            var responseData = await JsonDocument.ParseAsync(responseStream);

            ////Pega a propriedade "name" do JSON retornado como string. 
            //var name = responseData.RootElement.GetProperty("name").GetString();

            //// Verifica se não é nulo nem whitespace
            //name.ShouldNotBeNullOrWhiteSpace();

            //// Verifica se é igual ao nome esperado
            //name.ShouldBe(_name);

            responseData.RootElement.GetProperty("name").GetString().ShouldSatisfyAllConditions(
                name => name.ShouldNotBeNullOrWhiteSpace(),
                name => name.ShouldBe(_name)
                );

            responseData.RootElement.GetProperty("tokens").GetProperty("accessToken").GetString()
                .ShouldNotBeNullOrWhiteSpace();

        }

        [Theory]
        [ClassData(typeof(CultureInlineDataTest))]
        public async Task Error_Login_Invalid(string culture)
        {
            var request = RequestLoginJsonBuilder.Build();

            //  Envia a requisição POST para o endpoint "User" com o corpo da requisição como JSON
            var response = await DoPost(method, request, culture);

            // Verifica se o status da resposta é 400 Bad Request
            response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

            // Verifica se o conteúdo da resposta é do tipo application/json
            await using var responseStream = await response.Content.ReadAsStreamAsync();

            // Verifica se o conteúdo da resposta é do tipo application/json
            var responseData = await JsonDocument.ParseAsync(responseStream);

            // Pega a propriedade "errors" do JSON retornado como string.
            var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

            var expectedMessage = ResourceMessagesException.ResourceManager
                .GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(culture));

            // Verifica se a propriedade "errors" contém um erro específico
            errors.ShouldHaveSingleItem();

            // Verifica se o erro contém a mensagem esperada
            errors.ShouldContain(error => error.GetString()!.Equals(expectedMessage));
        }

    }
    
}
