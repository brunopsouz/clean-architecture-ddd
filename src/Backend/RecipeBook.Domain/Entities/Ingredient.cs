namespace RecipeBook.Domain
{
    public class Ingredient
    {
        public string Item { get; set; } = string.Empty;
        public long RecipeId { get; set; }

        //public Recipe? Recipe { get; set; } // Navigation property to the Recipe entity
    }
}