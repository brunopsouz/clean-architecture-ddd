using Azure.Core;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;
using System.Text.Json;
using Xunit;

namespace WebApi.Test.User.Profile
{
    public class GetUserProfileTest : RecipeBookClassFixture
    {
        private readonly string METHOD = "user";
        private readonly string _name;
        private readonly string _email;
        private readonly Guid _userIdentifier;

        public GetUserProfileTest(CustomWebApplicationFactory factory) : base(factory)
        {
            _name = factory.GetName();
            _email = factory.GetEmail();
            _userIdentifier = factory.GetUserIdentifier();

        }

        [Fact]
        public async Task Success()
        {
            var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

            var response = await DoGet(METHOD, token: token);

            response.StatusCode.ShouldBe(HttpStatusCode.OK);

            await using var responseBody = await response.Content.ReadAsStreamAsync();

            var responseData = await JsonDocument.ParseAsync(responseBody);

            responseData.RootElement.GetProperty("name").GetString().ShouldSatisfyAllConditions(
                name => name.ShouldNotBeNullOrWhiteSpace(),
                name => name.ShouldBe(_name)
                );

            responseData.RootElement.GetProperty("email").GetString().ShouldSatisfyAllConditions(
                name => name.ShouldNotBeNullOrWhiteSpace(),
                name => name.ShouldBe(_email)
                );
        }
    }
}
