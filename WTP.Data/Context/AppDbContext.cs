using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WTP.Api.Configuration;
using WTP.Domain.Entities;

namespace WTP.Data.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
            this.Database.Migrate();
        }

        public DbSet<Manager> Manager { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }

    }
}
