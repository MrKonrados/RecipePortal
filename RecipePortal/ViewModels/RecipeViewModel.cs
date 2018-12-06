using RecipePortal.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace RecipePortal.ViewModels
{
    public class RecipeViewModel
    {
        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        public string Directions { get; set; }

        public ICollection<Ingredient> Ingredients { get; set; }

        public HttpPostedFileBase File { get; set; }
    }
}