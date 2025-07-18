﻿using Bogus;
using RecipeBook.Communication.Enums;
using RecipeBook.Communication.Requests;

namespace CommonTestUtilities.Requests
{
    public class RequestRecipeJsonBuilder
    {
        public static RequestRecipeJson Build()
        {
            var step = 1;

            return new Faker<RequestRecipeJson>()
                .RuleFor(recipe => recipe.Title, (f) => f.Lorem.Word())
                .RuleFor(recipe => recipe.CookingTime, (f) => f.PickRandom<CookingTime>())
                .RuleFor(recipe => recipe.Difficulty, (f) => f.PickRandom<Difficulty>())
                .RuleFor(recipe => recipe.Ingredients, (f) => f.Make(3, () => f.Commerce.ProductName()))
                .RuleFor(recipe => recipe.DishTypes, (f) => f.Make(3, () => f.PickRandom<DishType>()))
                .RuleFor(recipe => recipe.Instructions, (f) => f.Make(3, () => new RequestInstructionJson
                {
                    Text = f.Lorem.Paragraph(),
                    Step = step++,
                }));




        }
    }
}
