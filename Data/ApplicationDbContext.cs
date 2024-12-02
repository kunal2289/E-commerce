using E_commerce.Models;
using Microsoft.EntityFrameworkCore;
namespace E_commerce.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}
