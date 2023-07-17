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
    public class StatisticsServiceTests : UnitTestsBase
    {
        private IStatisticsService statisticsService;

        [OneTimeSetUp]
        public void SetUp()
        {
            this.statisticsService = new StatisticsService(this.repo);
        }

        [Test]
        public async Task Total_ShouldReturnCorrectCounts()
        {
            //Arrange

            //Act: invoke the service method
            var result = await this.statisticsService.Total();

            //Assert the returned result is not null
            Assert.IsNotNull(result);

            //Assert that returned houses' count is correct
            var housesCount = await this.repo.AllReadonly<House>().CountAsync();
            Assert.That(result.TotalHouses, Is.EqualTo(housesCount));

            //Assert the returned rents' count is correct
            var rentsCount = await this.repo.AllReadonly<House>()
                .Where(h => h.RenterId != null).CountAsync();
            Assert.That(result.TotalRents, Is.EqualTo(rentsCount));
        }
    }
}
