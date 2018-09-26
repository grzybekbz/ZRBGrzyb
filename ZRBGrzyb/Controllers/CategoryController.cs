using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ZRBGrzyb.Models;

namespace ZRBGrzyb.Controllers {

    [Authorize]
    public class CategoryController : Controller {

        private IRepository repository;

        public CategoryController(IRepository repo) {

            repository = repo;
        }

        public ViewResult Index() => View(repository.Categories
                .OrderBy(p => p.Name));

        public ViewResult Create() => View("Edit", new Category());

        public ViewResult Edit(int categoryId) => View(repository.Categories
                .FirstOrDefault(p => p.CategoryID == categoryId));

        [HttpPost]
        public IActionResult Edit(Category category) {

            if (ModelState.IsValid) {

                category.RouteName = category.Name.Replace(" ","");

                //save to database
                repository.SaveCategory(category);
                TempData["message"] = $"Kategoria {category.Name} została zapisana";
                return RedirectToAction("Index");

            } else {

                return View(category);
            }
        }

        [HttpPost]
        public IActionResult Delete(int categoryId) {

            Category deletedCategory = repository.DeleteCategory(categoryId);

            if (deletedCategory != null) {
                TempData["message"] = $"Kategoria {deletedCategory.Name} została usunięta";

            } else {

                TempData["message"] = $"Kategoria nie może zostać usunięta";
            }

            return RedirectToAction("Index");
        }
    }
}