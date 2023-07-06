using HouseRentingSystem.Infrastructure.Common;
using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models;
using HouseRentingSystem.Core.Models.Agent;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Infrastructure.Data.Entities;
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
        private readonly IUserService userService;

        public HouseService(IRepository repo, IUserService userService)
        {
            this.repo = repo;
            this.userService = userService;
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
                    Address = h.Address
                })
                .Take(3)
                .ToListAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await this.repo.AllReadonly<House>()
                .AnyAsync(h => h.Id == id);
        }

        public async Task<HouseDetailsServiceModel> HouseDetailById(int id)
        {
            var house = await this.repo.AllReadonly<House>()
                .Where(h => h.Id == id)
                .Select( h =>  new HouseDetailsServiceModel()
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    Description = h.Description,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    Category = h.Category.Name,
                    IsRented = h.RenterId != null,
                    Agent = new AgentServiceModel()
                    {
                        PhoneNumber = h.Agent.PhoneNumber,
                        Email = h.Agent.User.Email
                    }
                })
                .FirstAsync();

            var agentId = (await this.repo.AllReadonly<House>()
                .Where(h => h.Id == id).Include(h => h.Agent).FirstAsync()).Agent.UserId;
            house.Agent.FullName = await this.userService.UserFullName(agentId);

            return house;
        }

        public async Task Edit(
            int houseId,
            string title,
            string address,
            string description,
            string imageUrl,
            decimal price,
            int categoryId)
        {
            var house = await this.repo.GetByIdAsync<House>(houseId);

            house.Title = title;
            house.Address = address;
            house.Description = description;
            house.ImageUrl = imageUrl;
            house.PricePerMonth = price;
            house.CategoryId = categoryId;

            await this.repo.SaveChangesAsync();
        }

        public async Task<bool> HasAgentWithId(int houseId, string currentUserId)
        {
            var house = await this.repo.GetByIdAsync<House>(houseId);
            var agent = await this.repo.AllReadonly<Agent>().FirstOrDefaultAsync(a => a.Id == house.AgentId);

            if (agent == null)
            {
                return false;
            }

            if (agent.UserId != currentUserId)
            {
                return false;
            }

            return true;
        }

        public async Task<int> GetHouseCategoryId(int houseId)
        {
            return (await this.repo.GetByIdAsync<House>(houseId)).CategoryId;
        }

        public async Task Delete(int houseId)
        {
            await this.repo.DeleteAsync<House>(houseId);
            await this.repo.SaveChangesAsync();
        }

        public async Task<bool> IsRented(int id)
        {
            return (await this.repo.GetByIdAsync<House>(id)).RenterId != null;
        }

        public async Task<bool> IsRentedByUserWithId(int houseId, string userId)  
        {
            var house = await this.repo.GetByIdAsync<House>(houseId);

            if (house == null)
            {
                return false;
            }

            if (house.RenterId != userId)
            {
                return false;
            }

            return true;
        }

        public async Task Rent(int houseId, string userId)
        {
            var house = await this.repo.GetByIdAsync<House>(houseId);

            house.RenterId = userId;
            await this.repo.SaveChangesAsync();
        }

        public async Task Leave(int houseId)
        {
            var house = await this.repo.GetByIdAsync<House>(houseId);

            house.RenterId = null;
            await this.repo.SaveChangesAsync();
        }
    }
}
