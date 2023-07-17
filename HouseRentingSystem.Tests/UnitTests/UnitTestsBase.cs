using HouseRentingSystem.Infrastructure.Common;
using HouseRentingSystem.Infrastructure.Data;
using HouseRentingSystem.Infrastructure.Data.Entities;
using HouseRentingSystem.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Tests.UnitTests
{
    public class UnitTestsBase
    {
        protected IRepository repo;

        [OneTimeSetUp]
        public void SetUpBase()
        {
            this.repo = new Repository(DatabaseMock.Instance);
            Task.Run(async () => await this.SeedDataBase());
        }

        [OneTimeTearDown]
        public void TearDownBase()
        {
            this.repo.Dispose();
        }

        public User Renter { get; private set; } = null!;

        public Agent Agent { get; private set; } = null!;

        public House RenterHouse { get; private set; } = null!; 

        private async Task SeedDataBase()
        {
            this.Renter = new User()
            {
                Id = "RenterUserId",
                Email = "rent@er.bg",
                FirstName = "Renter",
                LastName = "User"
            };
            await this.repo.AddAsync(this.Renter);

            this.Agent = new Agent()
            {
                PhoneNumber = "+359111111111",
                User = new User()
                {
                    Id = "TestUserId",
                    Email = "test@test.bg",
                    FirstName = "Test",
                    LastName = "Tester"
                }
            };
            await this.repo.AddAsync(this.Agent);

            this.RenterHouse = new House()
            {
                Title = "First Test House",
                Address = "Test, 201 Test",
                Description = "This is a test description. This is a test description. This is a test description.",
                ImageUrl = "https://www.bhg.com/thmb/0Fg0imFSA6HVZMS2DFWPvjbYDoQ=/1500x0/filters:no_upscale():max_bytes(150000):strip_icc()/white-modern-house-curved-patio-archway-c0a4a3b3-aa51b24d14d0464ea15d36e05aa85ac9.jpg",
                Renter = this.Renter,
                Agent = this.Agent,
                Category = new Category() { Name = "Cottage" },
            };
            await this.repo.AddAsync(this.RenterHouse);

            var nonRentedHouse = new House()
            {
                Title = "Second Test House",
                Address = "Test, 204 Test",
                Description = "This is another test description. This is another test description.",
                ImageUrl = "https://images.adsttc.com/media/images/629f/3517/c372/5201/650f/1c7f/large_jpg/hyde-park-house-robeson-architects_1.jpg?1654601149",
                Renter = this.Renter,
                Agent = this.Agent,
                Category = new Category() { Name = "Single-Family" }
            };

            await this.repo.AddAsync(nonRentedHouse);
            await this.repo.SaveChangesAsync();
        }
    }
}
