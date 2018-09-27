using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ZRBGrzyb.Models;

namespace ZRBGrzyb.Controllers {

    [Authorize]
    public class WorksListController : Controller {

        private IRepository repository;

        public WorksListController(IRepository repo) {

            repository = repo;
        }

        [AllowAnonymous]
        public ViewResult Show() {

            ViewBag.CurrentPage = "WorksListShow";
            ViewBag.Selected = "WorksList";
            return View(repository.Works
                .OrderBy(p => p.Date));
        }

        public ViewResult Index() => View(repository.Works
                .OrderBy(p => p.Date));

        public ViewResult Create() => View("Edit", new Work());

        public ViewResult Edit(int workId) => View(repository.Works
                .FirstOrDefault(p => p.WorkID == workId));

        [HttpPost]
        public IActionResult Edit(Work work) {

            if (ModelState.IsValid) {

                //save to database
                repository.SaveWork(work);
                TempData["message"] = $"Roboty zostały zapisane";
                return RedirectToAction("Index");

            } else {

                return View(work);
            }
        }

        [HttpPost]
        public IActionResult Delete(int workId) {

            Work deletedWork = repository.DeleteWork(workId);

            if (deletedWork != null)
                TempData["message"] = $"Roboty zostały usunięte";

            return RedirectToAction("Index");
        }
    }
}