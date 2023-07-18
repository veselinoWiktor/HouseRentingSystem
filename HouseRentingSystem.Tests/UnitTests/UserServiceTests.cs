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
    public class UserServiceTests : UnitTestsBase
    {
        private IUserService userService;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.userService = new UserService(this.repo, this.mapper);
        }

        [Test]
        public async Task UserHasRents_ShouldReturnTrue_WithValidData()
        {
            //Arrange

            //Act: invoke the service method with valid rented id
            var result = await this.userService.UserHasRents(this.Renter.Id);

            //Assert the returned value is true
            Assert.IsTrue(result);
        }

        [Test]
        public async Task UserFullName_ShouldReturnCorrectResult()
        {
            //Arrange

            //Act: invoke the service method with valid rented id
            var result = await this.userService.UserFullName(this.Renter.Id);

            //Assert the returned value is correct
            var renterFullName = this.Renter.FirstName + " " + this.Renter.LastName;
            Assert.That(result, Is.EqualTo(renterFullName));
        }

        [Test]
        public async Task All_ShouldReturnCorrectUsersAndAgents()
        {
            //Arrange

            //Act: invoke the service method
            var result = await this.userService.All();

            //Assert the returned users' count is correct
            var userCount = await this.repo.AllReadonly<User>().CountAsync();
            var resultUsers = result.ToList();
            Assert.That(resultUsers.Count(), Is.EqualTo(userCount));

            //Assert the returned agents' count is correct
            var agentsCount = await this.repo.AllReadonly<Agent>().CountAsync();
            var resultAgents = resultUsers.Where(us => us.PhoneNumber != "");
            Assert.That(resultAgents.Count(), Is.EqualTo(agentsCount));

            //Assert a returned agent data is correct
            var agentUser = resultAgents.FirstOrDefault(ag => ag.Email == this.Agent.User.Email);
            Assert.IsNotNull(agentUser);
            Assert.That(agentUser.PhoneNumber, Is.EqualTo(this.Agent.PhoneNumber));
        }
    }
}
