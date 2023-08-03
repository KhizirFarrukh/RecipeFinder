using Microsoft.EntityFrameworkCore;
using RecipeFinder.DAL.Entities;

namespace RecipeFinder.DAL
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> opt) : base(opt)
        {

        }

        public DbSet<Recipe> Recipes => Set<Recipe>();
    }
}
