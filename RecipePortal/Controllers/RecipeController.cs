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


            if (viewModel.file != null)
            {
                var file = viewModel.file;
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

        public ActionResult Detail(int? id)
        {
            if (id == null)
            {
                return HttpNotFound("Id should not be empty!");
            }

            var recipe = _context.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound("Recipe not exist in DB");
            }

            return View(recipe);
        }
    }
}