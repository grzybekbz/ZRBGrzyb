using System.Linq;

namespace ZRBGrzyb.Models {

    public class EFRepository : IRepository {

        private ApplicationDbContext context;

        public EFRepository(ApplicationDbContext ctx) {
            context = ctx;
        }

        public IQueryable<Photo> Photos => context.Photos;
        public IQueryable<Category> Categories => context.Categories;
        public IQueryable<Button> Buttons => context.Buttons;
        public IQueryable<Work> Works => context.Works;

        public void SavePhoto(Photo photo) {

            if (photo.PhotoID == 0) {

                context.Photos.Add(photo);

            } else {

                Photo dbEntry = context.Photos
                    .FirstOrDefault(p => p.PhotoID == photo.PhotoID);
                if (dbEntry != null) {

                    dbEntry.FileName = photo.FileName;
                    dbEntry.Description = photo.Description;
                    dbEntry.CategoryID = photo.CategoryID;
                    dbEntry.AddDate = photo.AddDate;
                }
            }
            context.SaveChanges();
        }

        public Photo DeletePhoto(int photoID) {

            Photo dbEntry = context.Photos
                .FirstOrDefault(p => p.PhotoID == photoID);
            if (dbEntry != null) {
                context.Photos.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public void SaveCategory(Category category) {

            if (category.CategoryID == 0) {

                context.Categories.Add(category);

            } else {

                Category dbEntry = context.Categories
                    .FirstOrDefault(p => p.CategoryID == category.CategoryID);
                if (dbEntry != null) {

                    dbEntry.Name = category.Name;
                    dbEntry.RouteName = category.RouteName;
                }
            }
            context.SaveChanges();
        }

        public Category DeleteCategory(int categoryID) {

            Category dbEntry = context.Categories
                .FirstOrDefault(p => p.CategoryID == categoryID);
            if (dbEntry != null && !dbEntry.Name.Equals("Brak")) {

                foreach (var photo in Photos.Where(p => p.CategoryID == categoryID)) {
                    photo.Category = Categories
                        .Where(c => c.Name == "Brak")
                        .FirstOrDefault();
                }
                context.Categories.Remove(dbEntry);
                context.SaveChanges();
                return dbEntry;

            } else {

                return null;
            }
        }

        public void SaveWork(Work work) {

            if (work.WorkID == 0) {

                context.Works.Add(work);

            } else {

                Work dbEntry = context.Works
                    .FirstOrDefault(p => p.WorkID == work.WorkID);
                if (dbEntry != null) {

                    dbEntry.Type = work.Type;
                    dbEntry.Value = work.Value;
                    dbEntry.Place = work.Place;
                    dbEntry.Date = work.Date;
                    dbEntry.Subject = work.Subject;
                }
            }
            context.SaveChanges();
        }

        public Work DeleteWork(int workID) {

            Work dbEntry = context.Works
                .FirstOrDefault(p => p.WorkID == workID);
            if (dbEntry != null) {
                context.Works.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public bool CheckFileExist(string fileName) {

            if(context.Photos.Any(p => p.FileName == fileName))
                return true;

            return false;
        }
    }
}
