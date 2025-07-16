using RecipeBook.Domain.Enums;

namespace RecipeBook.Domain.Dtos
{
    public record GenerateRecipeDto
    {
        public string Title { get; init; } = string.Empty;
        public IList<string> Ingredients { get; init; } = [];
        public IList<GeneratedInstructionDto> Instructions { get; init; } = [];
        public CookingTime CookingTime { get; init; }
    }
}
