using ConstrunctionApp.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using OnlineBookShop.Model;

namespace OnlineBookShop.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
       : base(options)
        {
        }
        public DbSet<Customer> customers { get; set; }
        public DbSet<User> users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _= modelBuilder.Entity<User>().HasData([
                new User{
                    Id = Guid.NewGuid(),
                    UserName ="admin",
                    Email="admin@gmail.com",
                    Password = "admin@1234",
                    Role ="Admin"
                }
                ]);
        }

    }
}
