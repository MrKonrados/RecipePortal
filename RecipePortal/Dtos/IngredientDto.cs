using System.ComponentModel.DataAnnotations;

namespace RecipePortal.Dtos
{
    public class IngredientDto
    {
        public int Id { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int? RecipeId { get; set; }

    }
}