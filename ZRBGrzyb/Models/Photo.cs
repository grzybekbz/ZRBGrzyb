using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZRBGrzyb.Models {

    public class Photo {
    
        public int PhotoID { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Wybierz plik")]
        public IFormFile FileToUpload { get; set; }

        [Required(ErrorMessage = "Nie wybrano pliku")]
        public string FileName { get; set; }

        [Required(ErrorMessage = "Podaj opis")]
        public string Description { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public Category Category { get; set; }

        [Required]
        [DataType(DataType.Date, ErrorMessage = "Wprowadź poprawną datę i godzinę")]
        public DateTime AddDate { get; set; }
    }
}