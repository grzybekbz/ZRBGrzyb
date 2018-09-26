using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ZRBGrzyb.Models;

namespace ZRBGrzyb.Components {

    public class GalleryCategoryMenu : ViewComponent {

        private IRepository repository;

        public GalleryCategoryMenu(IRepository repo) {

            repository = repo;
        }

        public IViewComponentResult Invoke() {

            ViewBag.SelectedCategory = RouteData?.Values["category"];
            return View(repository.Photos
                .Select(x => x.Category)
                .Distinct()
                .OrderByDescending(x => x.Name));
        }
    }
}
