using System.ComponentModel.DataAnnotations;

namespace RecipePortal.ViewModels
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}