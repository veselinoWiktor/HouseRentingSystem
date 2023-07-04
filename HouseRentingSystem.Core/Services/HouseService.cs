using BeautySaloon.Infrastructure.Data.Common;
using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Infrastucture.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Core.Services
{
    public class HouseService : IHouseService
    {
        private readonly IRepository repo;

        public HouseService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<HouseCategorySeviceModel>> AllCategories()
        {
            return await repo.AllReadonly<Category>()
                .Select(c => new HouseCategorySeviceModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                })
                .ToListAsync();
        }

        public async Task<bool> CategoryExists(int categoryId)
        {
            return await repo.AllReadonly<Category>()
                .AnyAsync(c => c.Id == categoryId);
        }

        public async Task<int> Create(string title, string address, string description, string imageUrl, decimal price, int categoryId, int agentId)
        {
            var house = new House()
            {
                Title = title,
                Address = address,
                Description = description,
                ImageUrl = imageUrl,
                PricePerMonth = price,
                CategoryId = categoryId,
                AgentId = agentId,
            };

            await this.repo.AddAsync(house);
            await this.repo.SaveChangesAsync();

            return house.Id;
        }

        public async Task<IEnumerable<HouseIndexServiceModel>> LastThreeHouses()
        {
            return await repo.AllReadonly<House>()
                .OrderByDescending(h => h.Id)
                .Select(h => new HouseIndexServiceModel()
                {
                    Id = h.Id,
                    Title = h.Title,
                    ImageUrl = h.ImageUrl,
                })
                .Take(3)
                .ToListAsync();
        }
    }
}
