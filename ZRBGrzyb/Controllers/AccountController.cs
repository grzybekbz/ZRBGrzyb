using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ZRBGrzyb.Models.ViewModels;
using ZRBGrzyb.Models;

namespace ZRBGrzyb.Controllers {

    [Authorize]
    public class AccountController : Controller {

        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;
        private readonly IUserValidator<AppUser> userValidator;
        private readonly IPasswordHasher<AppUser> passwordHasher;

        public AccountController(UserManager<AppUser> userMgr,
                                 SignInManager<AppUser> signInMgr,
                                 IUserValidator<AppUser> userValid,
                                 IPasswordHasher<AppUser> passwordHash,
                                 IConfiguration config) {

            userManager = userMgr;
            signInManager = signInMgr;
            userValidator = userValid;
            passwordHasher = passwordHash;

            IdentitySeedData.EnsurePopulated(userMgr, config).Wait();
        }

        [AllowAnonymous]
        public ViewResult Login(string returnUrl) {

            return View(new LoginModel {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel) {

            if (ModelState.IsValid) {
                AppUser user = await userManager.FindByNameAsync(loginModel.Name);
                if (user != null) {

                    await signInManager.SignOutAsync();
                    if ((await signInManager.PasswordSignInAsync(
                            user, loginModel.Password, false, false)).Succeeded) {

                        return Redirect(loginModel?.ReturnUrl ?? "/Admin/Index");
                    }
                }
            }
            ModelState.AddModelError("", "Nieprawidłowa nazwa lub hasło");
            return View(loginModel);
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/") {

            await signInManager.SignOutAsync();
            return Redirect(returnUrl);
        }

        public async Task<ViewResult> Index() => 
            View(await userManager.GetUserAsync(User));

        public async Task<IActionResult> Edit(string userId) {

            AppUser user = await userManager.FindByIdAsync(userId);

            if ( user != null) {

                return View(user);

            } else {

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id,
                string Email, string PhoneNumber) {

            AppUser user = await userManager.FindByIdAsync(id);

            if (user != null) {

                user.Email = Email;
                user.PhoneNumber = PhoneNumber;

                IdentityResult valid =
                        await userValidator.ValidateAsync(userManager, user);

                if (!valid.Succeeded) {

                    AddErrorsFromResult(valid);

                } else {

                    IdentityResult result = await userManager.UpdateAsync(user);

                    if (result.Succeeded) {

                        TempData["message"] = $"Dane użytkownika zostały zaktualizowane";
                        return RedirectToAction("Index");

                    } else {

                        AddErrorsFromResult(result);
                    }
                }
            } else {
                ModelState.AddModelError("", "Nie znaleziono użytkownika");
            }
            return View(user);
        }

        public IActionResult ChangePassword(string userId) {

            if (userId != null) {

                return View(new ChangePasswordModel { UserId = userId });

            } else {

                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel passwordModel) {

            AppUser user = await userManager.FindByIdAsync(passwordModel.UserId);

            if (user == null) {

                ModelState.AddModelError("", "Nie znaleziono użytkownika");

            } else if (ModelState.IsValid) {

                if (!passwordModel.NewPassword.Equals(passwordModel.RepeatNewPassword)) {

                    ModelState.AddModelError("", "Podane hasła nie są identyczne");

                } else {

                    IdentityResult result = await userManager.ChangePasswordAsync(
                        user, passwordModel.CurrentPassword, passwordModel.NewPassword);

                    if (result.Succeeded) {

                        TempData["message"] = $"Hasło użytkownika zostało zaktualizowane";
                        return RedirectToAction("Index");

                    } else {

                        AddErrorsFromResult(result);
                    }
                }
            }
            return View(passwordModel);
        }

        private void AddErrorsFromResult(IdentityResult result) {

            foreach (IdentityError error in result.Errors) {

                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
