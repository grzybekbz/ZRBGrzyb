using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace ZRBGrzyb.Models {

    public static class SeedData {

        public static void EnsurePopulated(IServiceProvider services) {

            ApplicationDbContext context = services.GetRequiredService<ApplicationDbContext>();

            if (!context.Categories.Any()) {
                context.Categories.AddRange(
                    new Category { Name = "Brak", RouteName = "Brak" },
                    new Category { Name = "Realizacje 2018", RouteName = "Realizacje2018" }
                );
                context.SaveChanges();
            }

            if (!context.Photos.Any()) {
                context.Photos.AddRange(
                    new Photo { FileName = "1.png", AddDate = new DateTime(2018, 5, 1, 8, 30, 52),
                        Category = context.Categories.First(), Description = "opis zdjecia numer 1",
                        CategoryID = context.Categories.First().CategoryID, 
                    },
                    new Photo { FileName = "2.png", AddDate = new DateTime(2018, 5, 1, 8, 30, 52),
                        Category = context.Categories.First(), Description = "opis zdjecia numer 2",
                        CategoryID = context.Categories.First().CategoryID
                    }
                );
                context.SaveChanges();
            }

            if (!context.Buttons.Any()) {
                context.Buttons.AddRange(
                    new Button() { Label = "Strona Główna", Controller = "Home", Action = "Index" },
                    new Button() { Label = "Galeria", Controller = "Photo", Action = "Gallery" },
                    new Button() { Label = "Oferta", Controller = "Home", Action = "Offer" },
                    new Button() { Label = "Praca", Controller = "Home", Action = "Job" },
                    new Button() { Label = "Kontakt", Controller = "Home", Action = "Contact" }
                );
                context.SaveChanges();
            };
        }
    }
}