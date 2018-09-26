using System.ComponentModel.DataAnnotations;

namespace ZRBGrzyb.Models.ViewModels {

    public class LoginModel {

        [Required]
        public string Name { get; set; }

        [Required]
        [UIHint("hasło")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }
}
