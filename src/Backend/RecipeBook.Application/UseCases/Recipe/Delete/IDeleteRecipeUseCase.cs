using RecipeBook.Communication.Responses;

namespace RecipeBook.Application.UseCases.Recipe.Delete
{
    public interface IDeleteRecipeUseCase
    {
        Task Execute(long recipeId);
    }
}