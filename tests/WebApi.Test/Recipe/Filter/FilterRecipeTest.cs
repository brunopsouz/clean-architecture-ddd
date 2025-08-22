using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using RecipeBook.Communication.Requests;
using RecipeBook.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;
using Xunit;

namespace WebApi.Test.Recipe.Filter;
public class FilterRecipeTest : RecipeBookClassFixture
{
    private const string METHOD = "recipe/filter";

    private readonly Guid _userIdentifier;

    private string _recipeTitle;
    private RecipeBook.Domain.Enums.Difficulty _recipedifficultyLevel;
    private RecipeBook.Domain.Enums.CookingTime _recipeCookingTime;
    private IList<RecipeBook.Domain.Enums.DishType> _recipeDishTypes;

    public FilterRecipeTest(CustomWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();

        _recipeTitle = factory.GetRecipeTitle();
        _recipeCookingTime = factory.GetRecipeCookingTime();
        _recipedifficultyLevel = factory.GetRecipeDifficulty();
        _recipeDishTypes = (IList<RecipeBook.Domain.Enums.DishType>?)factory.GetDishTypes();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestFilterRecipeJson
        {
            CookingTimes = [(RecipeBook.Communication.Enums.CookingTime)_recipeCookingTime],
            Difficulties = [(RecipeBook.Communication.Enums.Difficulty)_recipedifficultyLevel],
            DishTypes = _recipeDishTypes.Select(dishType => (RecipeBook.Communication.Enums.DishType)dishType).ToList(),
            RecipeTitle_Ingredient = _recipeTitle,
        };

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("recipes").EnumerateArray().ShouldNotBeEmpty();
    }

    [Fact]
    public async Task Success_NoContent()
    {
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.RecipeTitle_Ingredient = "recipeDontExist";

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPost(method: METHOD, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_CookingTime_Invalid(string culture)
    {
        var request = RequestFilterRecipeJsonBuilder.Build();
        request.CookingTimes.Add((RecipeBook.Communication.Enums.CookingTime)1000);

        var token = JwtTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoPost(method: METHOD, request: request, token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ResourceMessagesException.ResourceManager.GetString("COOKING_TIME_NOT_SUPPORTED", new CultureInfo(culture));

        errors.ShouldSatisfyAllConditions(
                name => name.ShouldHaveSingleItem(),
                name => name.ShouldContain(error => error.GetString()!.Equals(expectedMessage))
                );
    }
}