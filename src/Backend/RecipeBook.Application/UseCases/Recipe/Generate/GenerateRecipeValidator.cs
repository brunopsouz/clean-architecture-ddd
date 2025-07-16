using FluentValidation;
using RecipeBook.Communication.Requests;
using RecipeBook.Domain.ValueObjects;
using RecipeBook.Exceptions;

namespace RecipeBook.Application.UseCases.Recipe.Generate
{
    public class GenerateRecipeValidator : AbstractValidator<RequestGenerateRecipeJson>
    {
        public GenerateRecipeValidator()
        {
            var maximum_number_ingredients = RecipeBookRuleConstants.MAXIMUM_INGREDIENTS_GENERATE_RECIPE;

            RuleFor(request => request.Ingredients.Count)
                .InclusiveBetween(1, maximum_number_ingredients)
                .WithMessage(ResourceMessagesException.AT_LEAST_ONE_INGREDIENT);

            RuleFor(request => request.Ingredients)
                .Must(ingredients => ingredients.Count == ingredients.Distinct().Count())
                .WithMessage(ResourceMessagesException.DUPLICATED_INGREDIENTS_IN_LIST);

            RuleFor(request => request.Ingredients).ForEach(rule =>
            {
                rule.Custom((value, context) =>
                {
                    if (string.IsNullOrWhiteSpace(value))
                    {
                        context.AddFailure("Ingredient", ResourceMessagesException.INGREDIENT_EMPTY);
                    }
                    else if (value.Count(c => c == ' ') > 3 || value.Count(c => c == '/') > 1)
                    {
                        context.AddFailure("Ingredient", ResourceMessagesException.INGREDIENT_NOT_FOLLOWING_PATTERN);
                    }
                });
            });
        }
    }
}