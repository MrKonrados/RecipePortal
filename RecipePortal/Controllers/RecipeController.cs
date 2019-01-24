using AutoMapper;
using RecipePortal.Dtos;
using RecipePortal.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
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
            return View("RecipeForm", new RecipeDto());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecipeForm(RecipeDto dto, HttpPostedFileBase image)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (image != null)
            {
                var uploadDir = GlobalVariables.UploadDir;
                var fin = System.IO.Path.GetFileName(image.FileName);
                var path = System.IO.Path.Combine(Server.MapPath(uploadDir), fin);

                try
                {
                    image.SaveAs(path);
                }
                catch (Exception e)
                {
                    //todo: Pass error massage to view
                    return Content(e.ToString());
                }

                dto.ImageFilename = fin;
            }

            var orgRecipe = _context.Recipes
                .Where(r => r.Id == dto.Id)
                .Include(i => i.Ingredients)
                .SingleOrDefault();


            if (orgRecipe == null)
            {
                var newRecipe = Mapper.Map<Recipe>(dto);
                _context.Recipes.Add(newRecipe);
            }
            else
            {
                foreach (var ingredient in dto.Ingredients)
                {
                    var orgIng = orgRecipe.Ingredients
                        .SingleOrDefault(i => i.Id == ingredient.Id && i.Id != 0);
                    if (orgIng != null)
                    {
                        var itemEntry = _context.Entry(orgIng);
                        itemEntry.CurrentValues.SetValues(ingredient);
                    }
                    else
                    {
                        ingredient.Id = 0;
                        var newIng = Mapper.Map<Ingredient>(ingredient);
                        orgRecipe.Ingredients.Add(newIng);
                    }
                }

                var orgIngredients = orgRecipe.Ingredients.Where(i => i.Id != 0).ToList();
                foreach (var orgIng in orgIngredients)
                {
                    if (dto.Ingredients.All(i => i.Id != orgIng.Id))
                        _context.Ingredients.Remove(orgIng);
                }
            }

            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AddIngredient()
        {
            var ingredient = new IngredientDto();
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
            return View(recipe);
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

            recipe.ImageFilename = null;

            var dto = Mapper.Map<RecipeDto>(recipe);
            return View("RecipeForm", dto);
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
            var id = Int32.Parse(recipeId);

            var newRating = new Rating()
            {
                RecipeId = id,
                Score = Int32.Parse(rating_arr[1])
            };

            _context.Ratings.Add(newRating);
            _context.SaveChanges();
            var vm = _context.Recipes
                            .Include(i => i.Ingredients)
                            .SingleOrDefault(r => r.Id == id);
            return View("Detail", vm);
        }
    }
}