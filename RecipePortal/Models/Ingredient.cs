using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipePortal.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int RecipeId { get; set; }

        [ForeignKey(nameof(RecipeId))]
        public virtual Recipe Recipe { get; set; }
    }
}