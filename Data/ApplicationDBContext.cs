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
        public DbSet<Roles> roles {  get; set; }
        public DbSet<Privilege> privileges { get; set; }
        public DbSet<PrivilegeDetails> privilegeDetails { get; set; }
        public DbSet<Books> Book { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<SubCategory> SubCategory { get; set; }
        public DbSet<Orders> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Reviews> Reviews { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            _= modelBuilder.Entity<User>().HasData([
                new User{
                    Id = Guid.NewGuid(),
                    UserName ="admin",
                    UserCode="U001",
                    Email="admin@gmail.com",
                    Password = "admin@1234",
                    Role ="Admin"
                }
                ]);
        }

    }
}
