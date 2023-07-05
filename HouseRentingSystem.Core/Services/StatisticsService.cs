using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.Statistics;
using HouseRentingSystem.Infrastructure.Common;
using HouseRentingSystem.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.Core.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IRepository repo;

        public StatisticsService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task<StatisticsServiceModel> Total()
        {
            var totalHouses = await this.repo
                .AllReadonly<House>()
                .CountAsync();
            var totalRents = await this.repo
                .AllReadonly<House>()
                .Where(h => h.RenterId != null)
                .CountAsync();

            return new StatisticsServiceModel()
            {
                TotalHouses = totalHouses,
                TotalRents = totalRents,
            };
        }
    }
}
