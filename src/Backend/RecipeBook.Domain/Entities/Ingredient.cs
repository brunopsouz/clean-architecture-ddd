using RecipeBook.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeBook.Domain
{
    [Table("Ingredients")]
    public class Ingredient : EntityBase
    {
        public string Item { get; set; } = string.Empty;
        public long RecipeId { get; set; }

        //public Recipe? Recipe { get; set; } // Navigation property to the Recipe entity
    }
}