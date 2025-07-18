﻿namespace RecipeBook.Domain.Repositories.Recipe
{
    public interface IRecipeWriteOnlyRepository
    {
        public Task Add(Entities.Recipe recipe);

        public Task Delete(long recipeId);
    }
}
