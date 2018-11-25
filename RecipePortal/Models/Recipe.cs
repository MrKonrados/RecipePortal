using System.ComponentModel.DataAnnotations;

namespace RecipePortal.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public string Directions { get; set; }

        [StringLength(1024)]
        public string ImagePath { get; set; }
    }
}