using HouseRentingSystem.Tests.Mocks;
using HouseRentingSystem.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Tests.IntegrationTests
{
    public class StatisticsApiControllerTests
    {
        private StatisticsApiController statisticsController;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.statisticsController = new StatisticsApiController(StatisticsServiceMock.Instance);
        }

        [Test]
        public async Task GetStatistics_ShouldReturnCorrectCounts()
        {
            //Arrange

            //Act: invoke the service method
            var result = await this.statisticsController.GetStatistics();

            //Assert the returned result counts is correct
            Assert.NotNull(result);
            Assert.That(result.TotalHouses, Is.EqualTo(10));
            Assert.That(result.TotalRents, Is.EqualTo(6));
        }
    }
}
