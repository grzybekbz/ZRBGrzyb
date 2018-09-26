using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using ZRBGrzyb.Models;

namespace ZRBGrzyb.Controllers {

    [Authorize]
    public class AdminController : Controller {

        private readonly IRepository repository;

        public AdminController(IRepository repo) {

            repository = repo;
        }

        public ViewResult Index() => View();

        [HttpPost]
        public IActionResult SeedDatabase() {

            SeedData.EnsurePopulated(HttpContext.RequestServices);
            return RedirectToAction(nameof(Index));
        }
    }
}