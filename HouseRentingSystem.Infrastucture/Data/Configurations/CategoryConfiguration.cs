using HouseRentingSystem.Infrastucture.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseRentingSystem.Infrastucture.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasData(CreateCategories());
        }

        private static List<Category> CreateCategories()
        {
            return new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name = "Cottage"
                },
                new Category()
                {
                    Id = 2,
                    Name = "Single-Family"
                },
                new Category()
                {
                    Id = 3,
                    Name = "Duplex"
                }
            };
        }
    }
}
