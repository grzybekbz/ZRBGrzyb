using System.ComponentModel.DataAnnotations;

namespace ZRBGrzyb.Models {

    public class Category {

        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Podaj nazwę kategorii" )]
        public string Name { get; set; }

        public string RouteName { get; set; }
    }
}
