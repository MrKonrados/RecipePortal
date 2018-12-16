using RecipePortal.Models;
using RecipePortal.ViewModels;
using System;
using System.Linq;
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
            var recipes = _context.Recipes.ToList();
            return View(recipes);
        }

        // GET: Recipe
        public ActionResult Create()
        {
            var recipe = new RecipeViewModel();
            return View(recipe);
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
                Directions = viewModel.Directions,
                Ingredients = viewModel.Ingredients
            };


            if (viewModel.File != null)
            {
                var file = viewModel.File;
                var uploadDir = GlobalVariables.UploadDir;
                var fin = System.IO.Path.GetFileName(file.FileName);
                var path = System.IO.Path.Combine(Server.MapPath(uploadDir), fin);

                try
                {
                    file.SaveAs(path);
                }
                catch (Exception e)
                {
                    //todo: Pass error massage to view
                    return View();
                }
                recipe.ImageFilename = fin;
            }

            _context.Recipes.Add(recipe);
            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult AddIngredient()
        {
            var ingredient = new Ingredient();
            return PartialView("_Ingredient", ingredient);
        }

        public ActionResult Detail(int id = 0)
        {
            var recipe = _context.Recipes.SingleOrDefault<Recipe>(r => r.Id == id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            var ingredients = _context.Ingredients.Where(ing => ing.RecipeId == id).ToList();

            var viewModel = new RecipeViewModel
            {
                Directions = recipe.Directions,
                Name = recipe.Name,
                Ingredients = ingredients,
                //TODO: Display reciple image                
            };

            return View(viewModel);
        }
    }
}