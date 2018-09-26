using Microsoft.AspNetCore.Mvc;
using ZRBGrzyb.Models;

namespace ZRBGrzyb.Components {

    public class NavigationMenu : ViewComponent {

        private readonly IRepository repository;

        public NavigationMenu(IRepository repo) {

            repository = repo;
        }

        public IViewComponentResult Invoke() {

            return View(repository.Buttons);
        }
    }
}
