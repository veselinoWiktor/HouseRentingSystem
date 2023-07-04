using BeautySaloon.Infrastructure.Data.Common;
using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Infrastucture.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace HouseRentingSystem.Core.Services
{
    public class HouseService : IHouseService
    {
        private readonly IRepository repo;

        public HouseService(IRepository repo)
        {
            this.repo = repo;
        }

        public async Task<HouseQueryServiceModel> All(
            string? category = null,
            string? searchTerm = null,
            HouseSorting sorting = HouseSorting.Newest,
            int currentPage = 1,
            int housesPerPage = 1)
        {
            var houseQuery = this.repo.AllReadonly<House>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                houseQuery = houseQuery.Where(h => h.Category.Name == category);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm.ToLower()}%";
                houseQuery = houseQuery.Where(h =>
                    EF.Functions.Like(h.Title, searchTerm) ||
                    EF.Functions.Like(h.Address, searchTerm) ||
                    EF.Functions.Like(h.Description, searchTerm));
                    
            }

            houseQuery = sorting switch
            {
                HouseSorting.Price => houseQuery
                .OrderBy(h => h.PricePerMonth),
                HouseSorting.NotRentedFirst => houseQuery
                .OrderBy(h => h.RenterId != null)
                .ThenByDescending(h => h.Id),
                _ => houseQuery
                .OrderByDescending(h => h.Id)
            };

            var houses = await houseQuery
                .Skip((currentPage - 1) * housesPerPage)
                .Take(housesPerPage)
                .Select(h => new HouseServiceModel()
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth
                })
                .ToListAsync();

            var totalHouses = await houseQuery.CountAsync();

            return new HouseQueryServiceModel()
            {
                TotalHousesCount = totalHouses,
                Houses = houses
            };
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

        public async Task<IEnumerable<string>> AllCategoriesNames()
        {
            return await repo.AllReadonly<Category>()
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByAgentId(int agentId)
        {
            var houses = await this.repo.AllReadonly<House>()
                .Where(h => h.AgentId == agentId)
                .ToListAsync();

            return ProjectToModel(houses);
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByUserId(string userId)
        {
            var houses = await this.repo.AllReadonly<House>()
                .Where(h => h.RenterId == userId)
                .ToListAsync();

            return ProjectToModel(houses);
        }

        private List<HouseServiceModel> ProjectToModel(List<House> houses)
        {
            var resultHouses = houses
                .Select(h => new HouseServiceModel()
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId != null
                }).
                ToList();

            return resultHouses;
        }

        public async Task<bool> CategoryExists(int categoryId)
        {
            return await repo.AllReadonly<Category>()
                .AnyAsync(c => c.Id == categoryId);
        }

        public async Task<int> Create(
            string title,
            string address,
            string description,
            string imageUrl,
            decimal price,
            int categoryId,
            int agentId)
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
