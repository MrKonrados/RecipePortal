namespace RecipePortal.ViewModels
{
    public class RecipeEditViewModel : RecipeCreateViewModel
    {
        public int Id { get; set; }
        public string ImageFilename { get; set; }
    }
}