using HouseRentingSystem.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HouseRentingSystem.Infrastructure.Data.Configurations
{
    public class AgentConfiguration : IEntityTypeConfiguration<Agent>
    {
        public void Configure(EntityTypeBuilder<Agent> builder)
        {
            builder.HasData(CreateAgents());
        }

        private static List<Agent> CreateAgents()
        {
            return new List<Agent>()
            {
                new Agent()
                {
                    Id = 1,
                    PhoneNumber = "+359888888888",
                    UserId = "dea12856-c198-4129-b3f3-b893d8395082"
                },
                new Agent()
                {
                    Id = 5,
                    PhoneNumber = "+359123456789",
                    UserId = "bcb4f072-ecca-43c9-ab26-c060c6f364e4"
                }
            };
        }
    }
}
