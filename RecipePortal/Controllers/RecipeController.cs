using RecipePortal.Models;
using RecipePortal.ViewModels;
using System.Web.Mvc;

namespace RecipePortal.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecipeController()
        {
            _context = new ApplicationDbContext();
        }


        public ActionResult Index()
        {
            return View();
        }

        // GET: Recipe
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecipeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var recipe = new Recipe()
            {
                Name = viewModel.Name,
                Directions = viewModel.Directions
            };

            _context.Recipes.Add(recipe);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

    }
}