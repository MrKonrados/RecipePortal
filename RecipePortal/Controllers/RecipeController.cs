using RecipePortal.Models;
using RecipePortal.ViewModels;
using System;
using System.Data.Entity;
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
            var recipe = new CreateRecipeViewModel();
            return View(recipe);
            //            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateRecipeViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var recipe = new Recipe()
            {
                Name = viewModel.Name,
                AboutDish = viewModel.AboutDish,
                Ingredients = viewModel.Ingredients,
                Directions = viewModel.Directions
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
                    return Content(e.ToString());
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
            var recipe = _context.Recipes.Include(x => x.Ingredients).SingleOrDefault<Recipe>(r => r.Id == id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            var viewModel = new RecipeViewModel
            {
                Directions = recipe.AboutDish,
                Name = recipe.Name,
                AboutDish = recipe.AboutDish,
                Ingredients = recipe.Ingredients,
                ImageFilename = recipe.ImageFilename
            };
            return View(viewModel);
        }
    }
}