using CommonTestUtilities.Requests;
using RecipeBook.Application.UseCases.Recipe.Filter;
using RecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.Recipe.Filter
{
    public class FilterRecipeValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new FilterRecipeValidator();
            var request = RequestFilterRecipeJsonBuilder.Build();
            var result = validator.Validate(request);
            result.IsValid.ShouldBe(true);
        }

        [Fact]
        public void Error_Invalid_Cooking_Time()
        {
            var validator = new FilterRecipeValidator();

            var request = RequestFilterRecipeJsonBuilder.Build();
            request.CookingTimes.Add((RecipeBook.Communication.Enums.CookingTime)1000);

            var result = validator.Validate(request);

            result.IsValid.ShouldBe(false);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED);
        }

        [Fact]
        public void Error_Invalid_Difficulty()
        {
            var validator = new FilterRecipeValidator();

            var request = RequestFilterRecipeJsonBuilder.Build();
            request.Difficulties.Add((RecipeBook.Communication.Enums.Difficulty)1000);

            var result = validator.Validate(request);

            result.IsValid.ShouldBe(false);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED);
        }

        [Fact]
        public void Error_Invalid_Dish_Type()
        {
            var validator = new FilterRecipeValidator();
            var request = RequestFilterRecipeJsonBuilder.Build();
            request.DishTypes.Add((RecipeBook.Communication.Enums.DishType)1000);
            var result = validator.Validate(request);
            result.IsValid.ShouldBe(false);
            result.Errors.Count.ShouldBe(1);
            result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED);
        }
    }
}
