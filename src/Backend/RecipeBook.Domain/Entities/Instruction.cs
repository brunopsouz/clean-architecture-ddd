using RecipeBook.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeBook.Domain
{
    [Table("Instructions")]
    public class Instruction : EntityBase
    {
        public int Step { get; set; }
        public string Text { get; set; } = string.Empty;
        public long RecipeId { get; set; }

        //public Recipe? Recipe { get; set; } // Navigation property to the Recipe entity
    }
}