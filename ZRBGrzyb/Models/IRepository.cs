using System.Linq;

namespace ZRBGrzyb.Models {

    public interface IRepository {

        IQueryable<Photo> Photos { get; }
        IQueryable<Category> Categories { get; }
        IQueryable<Button> Buttons { get; }
        IQueryable<Work> Works { get; }

        void SavePhoto(Photo photo);
        Photo DeletePhoto(int photoID);

        void SaveCategory(Category category);
        Category DeleteCategory(int categoryID);

        void SaveWork(Work work);
        Work DeleteWork(int workID);

        bool CheckFileExist(string fileName);
    }
}
