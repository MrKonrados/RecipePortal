using System.ComponentModel.DataAnnotations;
using System.Web;

namespace RecipePortal.ViewModels
{
    public class CreateRecipeViewModel : RecipeViewModel
    {
        [Required]
        public HttpPostedFileBase File { get; set; }
    }
}