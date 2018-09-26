using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ZRBGrzyb.Models;

namespace ZRBGrzyb.Infrastructure {

    public class CustomUserValidator : UserValidator<AppUser> {

        public override async Task<IdentityResult> ValidateAsync(
                UserManager<AppUser> manager, AppUser user) {

            IdentityResult result = await base.ValidateAsync(manager, user);

            List<IdentityError> errors = result.Succeeded ? 
                new List<IdentityError>() : result.Errors.ToList();

            if (user.PhoneNumber.Any(n => char.IsLetter(n))) {

                errors.Add(new IdentityError {
                    Code = "PhoneNumberError",
                    Description = "Podany numer telefonu jest nieprawidłowy"
                });
            }

            if (!user.Email.Contains("@")) {

                errors.Add(new IdentityError {
                    Code = "EmailError",
                    Description = "Podany email jest nieprawidłowy"
                });
            }

            return errors.Count == 0 ? IdentityResult.Success
                    : IdentityResult.Failed(errors.ToArray());
        }
    }
}
