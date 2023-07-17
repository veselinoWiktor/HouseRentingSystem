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
    public class AgentServiceTests : UnitTestsBase
    {
        private IAgentService agentService;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.agentService = new AgentService(this.repo);
        }

        [Test]
        public async Task GetAgentId_ShouldReturnCorrectUserId()
        {
            //Arrange

            //Act: invoke the service method with valid id
            var resultAgentId = await this.agentService.GetAgentId(this.Agent.UserId);

            //Assert a correct id is returned
            Assert.That(this.Agent.Id, Is.EqualTo(resultAgentId));
        }

        [Test]
        public async Task ExistsById_ShouldReturnTrue_WithValidId()
        {
            //Arrange

            //Act: invoke the service method with valid id
            var result = await this.agentService.ExistsById(this.Agent.UserId);

            //Assert a correct id is returned
            Assert.IsTrue(result);
        }

        [Test]
        public async Task AgentWithPhoneNumberExists_ShouldReturnTrue_WithValidData()
        {
            //Arrange

            //Act: invoke the service method with valid id
            var result = await this.agentService
                .AgentWithPhoneNumberExists(this.Agent.PhoneNumber);

            //Assert a correct id is returned
            Assert.IsTrue(result);
        }

        [Test]
        public async Task CreateAgent_ShouldWorkCorrectly()
        {
            //Arrange: get all agents' current count
            var agentsCountBefore = await this.repo.AllReadonly<Agent>().CountAsync();

            //Act: invoke the service method with valid id
            await this.agentService.Create(this.Agent.UserId, this.Agent.PhoneNumber);

            //Assert the agents' count has increased by 1
            var agentsCountAfter = await this.repo.AllReadonly<Agent>().CountAsync();
            Assert.That(agentsCountAfter, Is.EqualTo(agentsCountBefore + 1));

            //Assert a new agent was created in the db with correct data
            var newAgentId = await this.agentService.GetAgentId(this.Agent.UserId);
            var newAgentInDb = await this.repo.GetByIdAsync<Agent>(newAgentId);
            Assert.IsNotNull(newAgentInDb);
            Assert.That(this.Agent.UserId, Is.EqualTo(newAgentInDb.UserId));
            Assert.That(this.Agent.PhoneNumber, Is.EqualTo(newAgentInDb.PhoneNumber));
        }
    }
}
