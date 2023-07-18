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
    public class RentServiceTests : UnitTestsBase
    {
        private IRentService rentService;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.rentService = new RentService(this.repo, this.mapper);
        }

        [Test]
        public async Task All_ShouldReturnCorrectData()
        {
            //Arrange

            //Act: invoke the service method
            var result = await rentService.All();

            //Assert the result is not null
            Assert.IsNotNull(result);

            //Assert the returned rents' count is correct
            var countOfRentedHousesInDb = await this.repo.AllReadonly<House>()
                .Where(h => h.RenterId != null).CountAsync();
            Assert.That(result.Count(), Is.EqualTo(countOfRentedHousesInDb));

            //Assert a returned rent's data is correct
            var resultHouse = result.ToList()
                .Find(h => h.HouseTitle == this.RenterHouse.Title);
            Assert.IsNotNull(resultHouse);

            Assert.That(resultHouse.RenterEmail, Is.EqualTo(this.Renter.Email));
            Assert.That(resultHouse.RenterFullName,
                Is.EqualTo(this.Renter.FirstName + " " + this.Renter.LastName));

            Assert.That(resultHouse.AgentEmail, Is.EqualTo(this.Agent.User.Email));
            Assert.That(resultHouse.AgentFullName,
                Is.EqualTo(this.Agent.User.FirstName + " " + this.Agent.User.LastName));
        }
    }
}
