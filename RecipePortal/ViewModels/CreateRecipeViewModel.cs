using System.Web;

namespace RecipePortal.ViewModels
{
    public class CreateRecipeViewModel : RecipeViewModel
    {
        public HttpPostedFileBase File { get; set; }

    }
}