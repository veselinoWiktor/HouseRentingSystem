using HouseRentingSystem.Infrastructure.Data.Configurations;
using HouseRentingSystem.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Infrastructure.Data
{
    public class HouseRentingDbContext : IdentityDbContext<User>
    {
        private bool seedDb;

        public HouseRentingDbContext(DbContextOptions<HouseRentingDbContext> options, bool seedDb = true)
            : base(options)
        {
            if (this.Database.IsRelational())
            {
                this.Database.Migrate();
            }
            else
            {
                this.Database.EnsureCreated();
            }

            this.seedDb = seedDb;
        }

        public DbSet<House> Houses { get; init; } = null!;

        public DbSet<Category> Categories { get; init; } = null!;

        public DbSet<Agent> Agents { get; init; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<House>()
                .Property(h => h.PricePerMonth)
                .HasPrecision(18, 2);

            builder
                .Entity<House>()
                .HasOne(h => h.Category)
                .WithMany(c => c.Houses)
                .HasForeignKey(h => h.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<House>()
                .HasOne(h => h.Agent)
                .WithMany()
                .HasForeignKey(h => h.AgentId)
                .OnDelete(DeleteBehavior.Restrict);

            if (this.seedDb)
            {
                builder.ApplyConfiguration(new UserConfiguration());
                builder.ApplyConfiguration(new AgentConfiguration());
                builder.ApplyConfiguration(new CategoryConfiguration());
                builder.ApplyConfiguration(new HouseConfiguration());
            }

            base.OnModelCreating(builder);
        }
    }
}