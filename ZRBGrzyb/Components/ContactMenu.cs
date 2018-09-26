using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using ZRBGrzyb.Models;

namespace ZRBGrzyb.Components {

    public class ContactMenu : ViewComponent {

        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration configuration;

        public ContactMenu(UserManager<AppUser> userMgr,
                           IConfiguration config) {

            userManager = userMgr;
            configuration = config;
        }

        public async Task<IViewComponentResult> InvokeAsync() {

            AppUser user = await userManager
                .FindByNameAsync(configuration["Data:Grzyb:AdminUser"]);

            ViewBag.Email = user.Email;
            ViewBag.PhoneNumber = user.PhoneNumber;
            ViewBag.NormalizedPhoneNumber = user.PhoneNumber.Replace(" ", "");

            return View();
        }
    }
}
