﻿using CommonTestUtilities.idEncryption;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;
using Xunit;

namespace WebApi.Test.Recipe.Update;
public class UpdateRecipeTestInvalidTokenTest : RecipeBookClassFixture
{
    private const string METHOD = "recipe";

    public UpdateRecipeTestInvalidTokenTest(CustomWebApplicationFactory webApplication) : base(webApplication)
    {
    }

    [Fact]
    public async Task Error_Token_Invalid()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var id = IdEncripterBuilder.Build().Encode(1);

        var response = await DoPut($"{METHOD}/{id}", request, token: "tokenInvalid");

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Without_Token()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var id = IdEncripterBuilder.Build().Encode(1);

        var response = await DoPut($"{METHOD}/{id}", request, token: string.Empty);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Error_Token_With_User_NotFound()
    {
        var request = RequestRecipeJsonBuilder.Build();

        var id = IdEncripterBuilder.Build().Encode(1);

        var token = JwtTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPut($"{METHOD}/{id}", request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
    }
}