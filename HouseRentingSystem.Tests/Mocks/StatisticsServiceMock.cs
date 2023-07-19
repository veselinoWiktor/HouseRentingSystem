using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.Statistics;
using HouseRentingSystem.Core.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Tests.Mocks
{
    public static class StatisticsServiceMock
    {
        public static IStatisticsService Instance
        {
            get
            {
                var statisticsServiceMock = new Mock<IStatisticsService>();

                statisticsServiceMock
                    .Setup(s => s.Total())
                    .ReturnsAsync(new StatisticsServiceModel()
                    {
                        TotalHouses = 10,
                        TotalRents = 6
                    });

                return statisticsServiceMock.Object;
            }
        }
    }
}
