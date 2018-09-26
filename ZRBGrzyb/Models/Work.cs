using System;
using System.ComponentModel.DataAnnotations;

namespace ZRBGrzyb.Models {

    public class Work {

        public int WorkID { get; set; }

        [Required(ErrorMessage = "Podaj typ robót")]
        public string Type { get; set; }

        [Required(ErrorMessage = "Podaj wartość robót")]
        public float Value { get; set; }

        [Required(ErrorMessage = "Podaj miejsce robót")]
        public string Place { get; set; }

        [DataType(DataType.Date, ErrorMessage = "Podaj datę")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Podaj zleceniodawcę")]
        public string Subject { get; set; }
    }
}
