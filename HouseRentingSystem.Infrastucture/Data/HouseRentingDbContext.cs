using HouseRentingSystem.Infrastucture.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Infrastucture.Data
{
    public class HouseRentingDbContext : IdentityDbContext
    {
        public HouseRentingDbContext(DbContextOptions<HouseRentingDbContext> options)
            : base(options)
        {
            this.Database.Migrate();
        }

        public DbSet<House> Houses { get; init; } = null!;

        public DbSet<Category> Categories { get; init; } = null!;

        public DbSet<Agent> Agents { get; init; } = null!;
    }
}