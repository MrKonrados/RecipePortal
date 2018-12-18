using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipePortal.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public string AboutDish { get; set; }

        [Required]
        public string Directions { get; set; }

        [StringLength(255)]
        public string ImageFilename { get; set; }

        [Required]
        public virtual ICollection<Ingredient> Ingredients { get; set; }
    }
}