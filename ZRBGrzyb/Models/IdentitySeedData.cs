using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace ZRBGrzyb.Models {

    public class IdentitySeedData {

        public static async Task EnsurePopulated(UserManager<AppUser> userManager,
                                                IConfiguration config) {

            string AdminUser = config["Data:Grzyb:AdminUser"];
            string AdminPassword = config["Data:Grzyb:AdminPassword"];
            string AdminEmail = config["Data:Grzyb:AdminEmail"];
            string AdminPhoneNumber = config["Data:Grzyb:AdminPhoneNumber"];

            AppUser user = await userManager.FindByIdAsync(AdminUser);

            if (user == null) {
                user = new AppUser { UserName = AdminUser,
                                     Email = AdminEmail,
                                     PhoneNumber = AdminPhoneNumber
                };
            };
            await userManager.CreateAsync(user, AdminPassword);
        }
    }
}
