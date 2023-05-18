
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Composition;

namespace Pizzeria.Models
{
    public class PizzaContext : IdentityDbContext<IdentityUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Pizzeria;Integrated Security=True; TrustServerCertificate=true");
        }
        public DbSet<Pizza> Pizze { get; set; }
        public DbSet<Categories> Categories { get; set; }
        public DbSet<Ingredient> ingredients { get; set; }


        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=Pizzeria;Integrated Security=True; TrustServerCertificate = true");
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
