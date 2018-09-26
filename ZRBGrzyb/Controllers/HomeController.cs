using Microsoft.AspNetCore.Mvc;

namespace ZRBGrzyb.Controllers {

    public class HomeController : Controller {

        public IActionResult Index() {

            ViewBag.CurrentPage = "HomeIndex";

            return View();
        }

        public IActionResult Offer() {

            ViewBag.CurrentPage = "HomeOffer";

            return View();
        }

        public IActionResult Job() {

            ViewBag.CurrentPage = "HomeJob";

            return View();
        }

        public IActionResult Contact() {

            ViewBag.CurrentPage = "HomeContact";

            return View();
        }
    }
}