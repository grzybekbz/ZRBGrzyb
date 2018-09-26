using Microsoft.EntityFrameworkCore;

namespace ZRBGrzyb.Models {

    public class ApplicationDbContext : DbContext {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Photo> Photos { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Button> Buttons { get; set; }
        public DbSet<Work> Works { get; set; }
    }
}
