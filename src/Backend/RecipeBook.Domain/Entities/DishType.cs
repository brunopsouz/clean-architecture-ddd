namespace RecipeBook.Domain.Entities
{
    public class DishType
    {
        public Enums.DishType Type { get; set; }
        public long RecipeId { get; set; }

        //public Recipe? Recipe { get; set; } // Navigation property to the Recipe entity
    }
}
