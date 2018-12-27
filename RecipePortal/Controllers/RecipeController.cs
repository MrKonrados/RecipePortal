using RecipePortal.Models;
using RecipePortal.ViewModels;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
            var recipe = new RecipeCreateViewModel();
            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecipeCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
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
            var recipe = _context.Recipes
                .Include(x => x.Ingredients)
                .Include(rat => rat.Ratings)
                .SingleOrDefault<Recipe>(r => r.Id == id);

            if (recipe == null)
            {
                return HttpNotFound();
            }

            var averageRating = 0.0;

            if (recipe.Ratings.Any())
                averageRating = (from rating in recipe.Ratings
                                 select rating.Score)
                                .Average();

            @ViewBag.AverageRating = decimal.Round((decimal)averageRating, 2);

            var viewModel = new RecipeViewModel
            {
                Id = recipe.Id,
                Directions = recipe.AboutDish,
                Name = recipe.Name,
                AboutDish = recipe.AboutDish,
                Ingredients = recipe.Ingredients,
                ImageFilename = recipe.ImageFilename
            };
            return View(viewModel);
        }

        public ActionResult Edit(int id = 0)
        {
            var recipe = _context.Recipes
                .Include(x => x.Ingredients)
                .SingleOrDefault(r => r.Id == id);

            if (recipe == null)
            {
                return HttpNotFound();
            }


            var viewModel = new RecipeEditViewModel
            {
                Id = recipe.Id,
                Directions = recipe.AboutDish,
                Name = recipe.Name,
                AboutDish = recipe.AboutDish,
                Ingredients = recipe.Ingredients,
                File = null
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RecipeEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var existingRecipe = _context.Recipes
                .Where(r => r.Id == viewModel.Id)
                .Include(r => r.Ingredients)
                .SingleOrDefault();

            if (existingRecipe == null)
            {
                return HttpNotFound();
            }

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
                viewModel.ImageFilename = fin;
            }


            if (existingRecipe != null)
            {
                _context.Entry(existingRecipe).CurrentValues.SetValues(viewModel);
                foreach (var existingIngredient in existingRecipe.Ingredients.ToList())
                {
                    if (!viewModel.Ingredients.Any(r => r.Id == existingIngredient.Id))
                        _context.Ingredients.Remove(existingIngredient);
                }

                foreach (var vmIngredient in viewModel.Ingredients)
                {
                    vmIngredient.RecipeId = viewModel.Id;
                    var existingIngredient = existingRecipe.Ingredients
                        .Where(r => r.Id == vmIngredient.Id)
                        .SingleOrDefault();

                    if (existingIngredient != null)
                    {
                        _context.Entry(existingIngredient)
                            .CurrentValues
                            .SetValues(vmIngredient);
                    }
                    else
                    {
                        var newIngredient = new Ingredient
                        {
                            RecipeId = vmIngredient.RecipeId,
                            Name = vmIngredient.Name,
                        };
                        existingRecipe.Ingredients.Add(newIngredient);
                    }
                }
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Recipe/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var recipe = _context.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }

            return View(recipe);
        }

        // POST: Recipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var recipe = _context.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            _context.Recipes.Remove(recipe);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RateRecipe(string star, string recipeId)
        {
            var rating_arr = star.Split('-');

            var newRating = new Rating()
            {
                RecipeId = Int32.Parse(recipeId),
                Score = Int32.Parse(rating_arr[1])
            };

            _context.Ratings.Add(newRating);
            _context.SaveChanges();

            return View("Detail");
        }
    }
}