using System.ComponentModel.DataAnnotations;

namespace ZRBGrzyb.Models.ViewModels {

    public class ChangePasswordModel {

        public string UserId { get; set; }

        [Required(ErrorMessage = "Podaj obecne hasło")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Podaj nowe hasło")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Powtórz nowe hasło")]
        public string RepeatNewPassword { get; set; }

    }
}
