using Microsoft.EntityFrameworkCore;
using Quickstart.AspNetCore.Data.Entities;

namespace Quickstart.AspNetCore
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<TGUser> TGUsers { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { 
            Database.EnsureCreated();
        }
    }
}