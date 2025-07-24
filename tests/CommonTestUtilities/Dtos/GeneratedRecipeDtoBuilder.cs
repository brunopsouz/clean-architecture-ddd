using Bogus;
using RecipeBook.Domain.Enums;
using RecipeBook.Domain.Dtos;

namespace CommonTestUtilities.Dtos;
public class GeneratedRecipeDtoBuilder
{
    public static GenerateRecipeDto Build()
    {
        return new Faker<GenerateRecipeDto>()
            .RuleFor(r => r.Title, faker => faker.Lorem.Word())
            .RuleFor(r => r.CookingTime, faker => faker.PickRandom<CookingTime>())
            .RuleFor(r => r.Ingredients, faker => faker.Make(1, () => faker.Commerce.ProductName()))
            .RuleFor(r => r.Instructions, faker => faker.Make(1, () => new GeneratedInstructionDto
            {
                Step = 1,
                Text = faker.Lorem.Paragraph()
            }));
    }
}
