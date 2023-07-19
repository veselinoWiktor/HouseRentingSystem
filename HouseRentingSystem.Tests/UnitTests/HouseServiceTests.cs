using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Services;
using HouseRentingSystem.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Tests.UnitTests
{
    [TestFixture]
    public class HouseServiceTests : UnitTestsBase
    {
        private IUserService userService;
        private IHouseService houseService;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.userService = new UserService(this.repo, this.mapper);
            this.houseService = new HouseService(this.repo, this.userService, this.mapper);
        }

        [Test]
        public async Task AllCategories_ShouldReturnCorrectCategories()
        {
            //Arrange

            //Act: invoke the service method
            var result = await this.houseService.AllCategories();

            //Assert the returned categories' count is correct
            var dbCategories = await this.repo.AllReadonly<Category>().ToListAsync();
            Assert.That(result.Count(), Is.EqualTo(dbCategories.Count));

            //Assert the returned categories are correct
            var categoryNames = dbCategories.Select(c => c.Name);
            Assert.That(categoryNames, Does.Contain(result.FirstOrDefault()?.Name));
        }

        [Test]
        public async Task All_ShouldReturnCorrectHouses()
        {
            //Arrange
            var searchTerm = "First";

            //Act: invoke the service method with the term
            var result = await this.houseService.All(null, searchTerm);

            //Assert the returned houses' count is correct
            var housesInDb = await this.repo.AllReadonly<House>()
                .Where(h => h.Title.Contains(searchTerm))
                .ToListAsync();
            Assert.That(result.TotalHousesCount, Is.EqualTo(housesInDb.Count));

            //Assert the returned house data is correct
            var resultHouse = result.Houses.FirstOrDefault();
            Assert.That(resultHouse, Is.Not.Null);

            var houseInDb = housesInDb.FirstOrDefault();
            Assert.Multiple(() =>
            {
                Assert.That(resultHouse.Id, Is.EqualTo(houseInDb?.Id));
                Assert.That(resultHouse.Title, Is.EqualTo(houseInDb?.Title));
            });
        }

        [Test]
        public async Task AllCategoryNames_ShouldReturnCorrectResult()
        {
            //Arrange

            //Act: invoke the service method
            var result = await this.houseService.AllCategoriesNames();

            //Assert the returned categories' count is correct
            var dbCategories = await this.repo.AllReadonly<Category>().ToListAsync();
            Assert.That(result.Count(), Is.EqualTo(dbCategories.Count));

            //Assert the returned categories' names are correct
            var categoryNames = dbCategories.Select(c => c.Name);
            Assert.That(categoryNames, Does.Contain(result.FirstOrDefault()));
        }

        [Test]
        public async Task AllHousesByAgentId_ShouldReturnCorrectHouses()
        {
            //Arrange: add a valid agent id to a variable
            var agentId = this.Agent.Id;

            //Act: invoke the service method with valid agent id
            var result = await this.houseService.AllHousesByAgentId(agentId);

            //Assert the returned result is not null
            Assert.That(result, Is.Not.Null);

            //Assert the returned houses' count is correct
            var housesInDb = await this.repo.AllReadonly<House>()
                .Where(h => h.AgentId == agentId)
                .ToListAsync();
            Assert.That(result.Count(), Is.EqualTo(housesInDb.Count));
        }

        [Test]
        public async Task AllHousesByUserId_ShouldReturnCorrectHouses()
        {
            //Arrange: add a valid renter id to a variable
            var renterId = this.Renter.Id;

            //Act: invoke the service method with valid renter id
            var result = await this.houseService.AllHousesByUserId(renterId);

            //Assert the returned result is not null
            Assert.That(result, Is.Not.Null);

            //Assert the returned houses' count is correct
            var housesInDb = await this.repo.AllReadonly<House>()
                .Where(h => h.RenterId == renterId)
                .ToListAsync();
            Assert.That(result.Count(), Is.EqualTo(housesInDb.Count));
        }

        [Test]
        public async Task HouseDetailsById_ShouldReturnCorrectHouseData()
        {
            //Arrange: get a valid rented house id
            var houseId = this.RenterHouse.Id;

            //Act: invoke the service method with the valid id
            var result = await this.houseService.HouseDetailById(houseId);

            //Assert the returned result is not null
            Assert.That(result, Is.Not.Null);

            //Assert the returned result data is correct
            var houseInDb = await this.repo.GetByIdAsync<House>(houseId);
            Assert.That(result.Id, Is.EqualTo(houseInDb.Id));
            Assert.That(result.Title, Is.EqualTo(houseInDb.Title));
        }

        [Test]
        public async Task Create_ShouldCreateHouse()
        {
            //Arrange: get the houses current cound
            var housesInDbBefore = await this.repo.AllReadonly<House>().CountAsync();

            //Arrange: create a new House variable with needed data
            var newHouse = new House()
            {
                Title = "New House",
                Address = "In a Galaxy far far away...",
                Description = "On a very hot sandy planet, in the outskirts of the capital city",
                ImageUrl = "https://www.pexels.com/photo/house-lights-turned-on-106399/"
            };

            //Act: invoke the serice method with neccessary valid data
            var newHouseId = await this.houseService.Create(newHouse.Title,
                newHouse.Address, newHouse.Description, newHouse.ImageUrl, 2200.00M, 1, this.Agent.Id);

            //Assert the houses current count increased by 1
            var housesInDbAfter = await this.repo.AllReadonly<House>().CountAsync();
            Assert.That(housesInDbAfter, Is.EqualTo(housesInDbBefore + 1));

            //Assert the new house is created with correct data
            var newHouseInDb = await this.repo.GetByIdAsync<House>(newHouseId);
            Assert.That(newHouseInDb.Title, Is.EqualTo(newHouse.Title));
        }

        [Test]
        public async Task CategoryExists_ShouldReturnTrue_WithValidId()
        {
            //Arrange: get a valid category id
            var categoryId = (await this.repo.AllReadonly<Category>().FirstAsync()).Id;


            //Act: invoke the service method with valid id
            var result = await this.houseService.CategoryExists(categoryId);

            //Assert the returned result is true
            Assert.IsTrue(result);
        }

        [Test]
        public async Task HasAgentWithId_ShouldReturnTrue_WithValidId()
        {
            //Arrange: get valid rented house's renter and agent ids
            var houseId = this.RenterHouse.Id;
            var userId = this.RenterHouse.Agent.User.Id;

            //Act: invoke the service method with valid ids
            var result = await this.houseService.HasAgentWithId(houseId, userId);

            //Assert the returned result is true
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetHouseCategoryId_ShouldReturnCorrectId()
        {
            //Arrange: get valid rented house's renter id
            var houseId = this.RenterHouse.Id;

            //Act: invoke the service method with valid id
            var result = await this.houseService.GetHouseCategoryId(houseId);

            //Assert the returned result is not null    
            Assert.IsNotNull(result);

            //Assert the returned category is correct
            var categoryId = this.RenterHouse.Category.Id;
            Assert.That(result, Is.EqualTo(categoryId));
        }

        [Test]
        public async Task IsRentedByUserWithId_ShouldReturnTrue_WithValidId()
        {
            //Arrange: get valid rented house and renter ids
            var houseId = this.RenterHouse.Id;
            var renterId = this.RenterHouse.Renter.Id;

            //Act: invoke the service method with valid ids
            var result = await this.houseService.IsRentedByUserWithId(houseId, renterId);

            //Assert the returned result is true
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Edit_ShouldEditHouseCorrectly()
        {
            //Arrange: add a new house to the database
            var house = new House()
            {
                Title = "New House for Edit",
                Address = "Sofia",
                Description = "This house is a test house that must be edited",
                ImageUrl = "https://www.pexels.com/photo/house-lights-turned-on-106399/"
            };

            await this.repo.AddAsync(house);
            await this.repo.SaveChangesAsync();

            //Arrange create a variable with a changed address
            var changedAddress = "Sofia, Bulgaria";

            //Act: invoke the serice method with valid and changed address
            await this.houseService.Edit(house.Id, house.Title,
                changedAddress, house.Description, house.ImageUrl, house.PricePerMonth, house.CategoryId);

            //Assert the house data in the database is correct
            var newHouseInDb = await this.repo.GetByIdAsync<House>(house.Id);
            Assert.That(newHouseInDb, Is.Not.Null);
            Assert.That(newHouseInDb.Title, Is.EqualTo(house.Title));
            Assert.That(newHouseInDb.Address, Is.EqualTo(changedAddress));
        }

        [Test]
        public async Task IsRented_ShouldReturnTrue_WithValidId()
        {
            //Arrange: get valid rented house id
            var houseId = this.RenterHouse.Id;

            //Act: invoke the service method with valid ids
            var result = await this.houseService.IsRented(houseId);

            //Assert the returned result is true
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Delete_ShouldDeleteHouseSuccessfully()
        {
            //Arrange: add a new house to the database
            var house = new House()
            {
                Title = "New House for delete",
                Address = "Sofia",
                Description = "This house is a test house that must be deleted",
                ImageUrl = "https://www.pexels.com/photo/house-lights-turned-on-106399/"
            };

            await this.repo.AddAsync(house);
            await this.repo.SaveChangesAsync();

            //Arrange get current houses' count
            var housesCountBefore = await this.repo.AllReadonly<House>().CountAsync();

            //Act: invoke the serice method with valid and changed address
            await this.houseService.Delete(house.Id);

            //Assert the returned houses' count is decreased by 1
            var houseCountAfter = await this.repo.AllReadonly<House>().CountAsync();
            Assert.That(houseCountAfter, Is.EqualTo(housesCountBefore - 1));

            //Assert the house data is not present in the db
            var houseInDb = await this.repo.GetByIdAsync<House>(house.Id);
            Assert.That(houseInDb, Is.Null);
        }

        [Test]
        public async Task Exists_ShouldReturnTrue_WithValidId()
        {
            //Arrange: get valid rented house id
            var houseId = this.RenterHouse.Id;

            //Act: invoke the service method with valid ids
            var result = await this.houseService.Exists(houseId);

            //Assert the returned result is true
            Assert.IsTrue(result);
        }

        [Test]
        public async Task Leave_ShouldReturnHouseSuccessfully()
        {
            //Arrange: add a new house to the database
            var house = new House()
            {
                Title = "New House for leave",
                RenterId = "TestRenterId",
                Address = "Somewhere in the middle of nowhere",
                Description = "This house is a test house that must be left",
                ImageUrl = "https://www.pexels.com/photo/house-lights-turned-on-106399/"
            };

            await this.repo.AddAsync(house);
            await this.repo.SaveChangesAsync();

            //Act: invoke the serice method with valid and changed address
            await this.houseService.Leave(house.Id);

            //Assert the renter id is null
            Assert.IsNull(house.RenterId);

            //Assert the house has a correct data in the db
            var houseInDb = await this.repo.GetByIdAsync<House>(house.Id);
            Assert.That(houseInDb, Is.Not.Null);
            Assert.That(houseInDb.RenterId, Is.Null);
        }

        [Test]
        public async Task Rent_ShouldReturnHouseSuccessfully()
        {
            //Arrange: add a new house to the database
            var house = new House()
            {
                Title = "New House for rent",
                Address = "a little to the left from the middle of nowhere",
                Description = "This house is a test house that must be rented",
                ImageUrl = "https://www.pexels.com/photo/house-lights-turned-on-106399/"
            };

            await this.repo.AddAsync(house);
            await this.repo.SaveChangesAsync();

            //Arrange get a valid renter id
            var renterId = this.Renter.Id;

            //Act: invoke the serice method with valid and changed address
            await this.houseService.Rent(house.Id, renterId);

            //Assert the house has a correct data in the db
            var houseInDb = await this.repo.GetByIdAsync<House>(house.Id);
            Assert.That(houseInDb, Is.Not.Null);
            Assert.That(renterId, Is.EqualTo(houseInDb.RenterId));
        }

        [Test]
        public async Task LastThreeHouses_ShouldReturnCorrectHouses()
        {
            //Arrange

            //Act: invoke the serice method
            var result = await this.houseService.LastThreeHouses();

            //Assert the returned houses count is correct
            var housesInDb = await this.repo.AllReadonly<House>()
                .OrderByDescending(h => h.Id)
                .Take(3)
                .ToArrayAsync();
            Assert.That(result.Count(), Is.EqualTo(housesInDb.Count()));


            //Assert the returned houses data is correct
            var firstHouseInDb = housesInDb.FirstOrDefault();

            var firstResultHouse = result.FirstOrDefault();
            Assert.That(firstResultHouse?.Id, Is.EqualTo(firstHouseInDb?.Id));
            Assert.That(firstResultHouse?.Title, Is.EqualTo(firstHouseInDb?.Title));
        }
    }
}
