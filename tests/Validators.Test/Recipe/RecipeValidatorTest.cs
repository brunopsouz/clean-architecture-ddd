using CommonTestUtilities.Requests;
using RecipeBook.Application.UseCases.Recipe;
using RecipeBook.Application.UseCases.Recipe.Register;
using RecipeBook.Communication.Enums;
using RecipeBook.Exceptions;
using Shouldly;

namespace Validators.Test.Recipe
{
    public class RecipeValidatorTest
    {
        [Fact]
        public void Success()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();

            var result = validator.Validate(request);

            result.IsValid.ShouldBe(true);
        }

        [Fact]
        public void Success_Cooking_Time_Null()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.CookingTime = null;

            var result = validator.Validate(request);

            result.IsValid.ShouldBe(true);
        }

        [Fact]
        public void Success_Difficulty_Null()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Difficulty = null;

            var result = validator.Validate(request);

            result.IsValid.ShouldBe(true);
        }

        [Fact]
        public void Error_Invalid_Cooking_Time()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.CookingTime = (CookingTime?)999;

            var result = validator.Validate(request);

            result.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBe(false),
                () => result.Errors.Count.ShouldBe(1),
                () => result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.COOKING_TIME_NOT_SUPPORTED));

        }

        [Fact]
        public void Error_Invalid_Difficulty()
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Difficulty = (Difficulty?)999;

            var result = validator.Validate(request);

            result.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBe(false),
                () => result.Errors.Count.ShouldBe(1),
                () => result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.DIFFICULTY_LEVEL_NOT_SUPPORTED));

        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Error_Invalid_Recipe_Title(string title)
        {
            var validator = new RecipeValidator();

            var request = RequestRecipeJsonBuilder.Build();
            request.Title = title;

            var result = validator.Validate(request);

            result.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBe(false),
                () => result.Errors.Count.ShouldBe(1),
                () => result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.RECIPE_TITLE_EMPTY));

        }

        [Fact]
        public void Success_DishType_Empty()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.DishTypes.Clear();

            var validator = new RecipeValidator();
            
            var result = validator.Validate(request);

            result.IsValid.ShouldBe(true);
        }

        [Fact]
        public void Error_Invalid_DishType()
        { 
            var request = RequestRecipeJsonBuilder.Build();
            request.DishTypes.Add((DishType)999);

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBe(false),
                () => result.Errors.Count.ShouldBe(1),
                () => result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.DISH_TYPE_NOT_SUPPORTED));

        }

        [Fact]
        public void Error_Empty_Ingredients()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Ingredients.Clear();

            var validator = new RecipeValidator();
            
            var result = validator.Validate(request);

            result.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBe(false),
                () => result.Errors.Count.ShouldBe(1),
                () => result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.AT_LEAST_ONE_INGREDIENT));
        }

        [Fact]
        public void Error_Empty_Instructions()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.Clear();

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBe(false),
                () => result.Errors.Count.ShouldBe(1),
                () => result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.AT_LIST_ONE_INSTRUCTION));
        }


        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Error_Empty_Value_Ingredients(string ingredient)
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Ingredients.Add(ingredient);

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBe(false),
                () => result.Errors.Count.ShouldBe(1),
                () => result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.INGREDIENT_EMPTY));
        }

        [Fact]
        public void Error_Same_Step_Instructions()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Step = request.Instructions.Last().Step;

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBe(false),
                () => result.Errors.Count.ShouldBe(1),
                () => result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.TWO_OR_MORE_INSTRUCTIONS_SAME_ORDER));
        }

        [Fact]
        public void Error_Non_Negative_Or_Zero_Step_Instructions()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Step = -1;

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBe(false),
                () => result.Errors.Count.ShouldBe(1),
                () => result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.NON_NEGATIVE_OR_ZERO_INSTRUCTION_STEP));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public void Error_Empty_Value_Instructions(string instructionText)
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Text = instructionText;

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBe(false),
                () => result.Errors.Count.ShouldBe(1),
                () => result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.INSTRUCTION_EMPTY));
        }

        [Fact]
        public void Error_Instruction_Exceeds_Limit_Characters()
        {
            var request = RequestRecipeJsonBuilder.Build();
            request.Instructions.First().Text = RequestStringGenerator.Paragraphs(minCharacters: 2001);

            var validator = new RecipeValidator();

            var result = validator.Validate(request);

            result.ShouldSatisfyAllConditions(
                () => result.IsValid.ShouldBe(false),
                () => result.Errors.Count.ShouldBe(1),
                () => result.Errors[0].ErrorMessage.ShouldBe(ResourceMessagesException.INSTRUCTION_EXCEEDS_LIMIT_CHARACTERS));
        }
    }
}
