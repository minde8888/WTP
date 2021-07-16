using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WTP.Api.Configuration;
using WTP.Domain.Entities;

namespace WTP.Data.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Manager> Manager { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }

    }
}
