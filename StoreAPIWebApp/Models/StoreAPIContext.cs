using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace StoreAPIWebApp.Models
{
    public class StoreAPIContext : DbContext
    {
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientCascade;
            }
        }

        public StoreAPIContext(DbContextOptions<StoreAPIContext> options)
        : base(options)
        {

        }
    }
}
