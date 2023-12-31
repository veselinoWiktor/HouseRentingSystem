﻿using BeautySaloon.Infrastructure.Data.Common;
using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.Agent;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Infrastructure.Data.Entities;
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
        private readonly IRepository repository;

        public HouseService(IRepository _repository)
        {
            repository = _repository;
        }

        public async Task<HouseQueryServiceModel> All(string? category = null, string? searchTerm = null, HouseSorting sorting = HouseSorting.Newest, int currentPage = 1, int housesPerPage = 1)
        {
            var housesQuery = repository.AllReadonly<House>();

            if (!String.IsNullOrWhiteSpace(category))
            {
                housesQuery = housesQuery
                    .Where(h => h.Category.Name == category);
            }

            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                housesQuery = housesQuery.Where(h =>
                    h.Title.ToLower().Contains(searchTerm.ToLower()) ||
                    h.Address.ToLower().Contains(searchTerm.ToLower()) ||
                    h.Description.ToLower().Contains(searchTerm.ToLower()));
            }

            housesQuery = sorting switch
            {
                HouseSorting.Price => housesQuery
                    .OrderBy(h => h.PricePerMonth),
                HouseSorting.NotRentedFirst => housesQuery
                    .OrderBy(h => h.RenterId != null)
                    .ThenByDescending(h => h.Id),
                _ => housesQuery
                    .OrderByDescending(h => h.Id)
            };

            var houses = await housesQuery
                .Skip((currentPage - 1) * housesPerPage)
                .Take(housesPerPage)
                .Select(h => new HouseServiceModel
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    ImageUrl = h.ImageUrl,
                    IsRented = h.RenterId != null,
                    PricePerMonth = h.PricePerMonth
                })
                .ToListAsync();

            var totalHouses = housesQuery.Count();

            return new HouseQueryServiceModel()
            {
                TotalHousesCount = totalHouses,
                Houses = houses
            };
        }

        public async Task<IEnumerable<HouseCategoryServiceModel>> AllCategories()
        {
            return await repository.AllReadonly<Category>()
                .Select(c => new HouseCategoryServiceModel()
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> AllCategoriesNames()
        {
            return await repository.AllReadonly<Category>()
                .Select(c => c.Name)
                .Distinct()
                .ToListAsync();
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByAgentId(int agentId)
        {
            var houses = await repository.AllReadonly<House>()
                .Where(h => h.AgentId == agentId)
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
                })
                .ToList();

            return resultHouses;
        }

        public async Task<IEnumerable<HouseServiceModel>> AllHousesByUserId(string userId)
        {
            var houses = await repository.AllReadonly<House>()
                .Where(h => h.RenterId == userId)
                .ToListAsync();

            return ProjectToModel(houses);
        }

        public async Task<bool> CategoryExists(int categoryId)
        {
            return await repository.AllReadonly<Category>()
                .AnyAsync(c => c.Id == categoryId);
        }

        public async Task<int> Create(string title, string address, string description, string imageUrl, decimal price, int categoryId, int agentId)
        {
            var house = new House
            {
                Title = title,
                Address = address,
                Description = description,
                ImageUrl = imageUrl,
                PricePerMonth = price,
                CategoryId = categoryId,
                AgentId = agentId
            };

            await repository.AddAsync(house);
            await repository.SaveChangesAsync();

            return house.Id;
        }

        public async Task<IEnumerable<HouseIndexServiceModel>> LastThreeHouses()
        {
            return await repository.AllReadonly<House>()
                .OrderByDescending(h => h.Id)
                .Select(h => new HouseIndexServiceModel
                {
                    Id = h.Id,
                    Title = h.Title,
                    ImageUrl = h.ImageUrl
                })
                .Take(3)
                .ToListAsync();
        }

        public async Task<bool> Exists(int id)
        {
            return await repository.AllReadonly<House>()
                .AnyAsync(h => h.Id == id);
        }

        public async Task<HouseDetailsServiceModel> HouseDetailsById(int id)
        {
            return await repository.AllReadonly<House>()
                .Where(h => h.Id == id)
                .Select(h => new HouseDetailsServiceModel()
                {
                    Id = h.Id,
                    Title = h.Title,
                    Address = h.Address,
                    Description = h.Description,
                    ImageUrl = h.ImageUrl,
                    PricePerMonth = h.PricePerMonth,
                    IsRented = h.RenterId != null,
                    Category = h.Category.Name,
                    Agent = new AgentServiceModel()
                    {
                        PhoneNumber = h.Agent.PhoneNumber,
                        Email = h.Agent.User.Email
                    }
                })
                .FirstAsync();
        }

        public async Task Edit(int houseId, string title, string address, string description, string imageUrl, decimal price, int categoryId)
        {
            var house = await repository.All<House>()
                .FirstAsync(h => h.Id == houseId);

            house.Title = title;
            house.Address = address;
            house.Description = description;
            house.ImageUrl = imageUrl;
            house.PricePerMonth = price;
            house.CategoryId = categoryId;

            await repository.SaveChangesAsync();
        }

        public async Task<bool> HasAgentWithId(int houseId, string currentUserId)
        {
            var house = await repository.GetByIdAsync<House>(houseId);
            var agent = await repository.AllReadonly<Agent>().FirstOrDefaultAsync(a => a.Id == house.AgentId);

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
            return (await repository.AllReadonly<House>().FirstAsync(h => h.Id == houseId)).CategoryId;
        }

        public async Task Delete(int houseId)
        {
            await repository.DeleteAsync<House>(houseId);
            await repository.SaveChangesAsync();
        }

        public async Task<bool> IsRented(int id)
        {
            return (await repository.GetByIdAsync<House>(id)).RenterId != null;
        }

        public async Task<bool> IsRentedByUserWithId(int houseId, string userId)
        {
            var house = await repository.GetByIdAsync<House>(houseId);

            if (house == null)
            {
                return false;
            }

            if (house.RenterId !=  userId)
            {
                return false;
            }

            return true;
        }

        public async Task Rent(int houseId, string userId)
        {
            var house = await repository.GetByIdAsync<House>(houseId);

            house.RenterId = userId;
            await repository.SaveChangesAsync();
        }

        public async Task Leave(int houseId)
        {
            var house = await repository.GetByIdAsync<House>(houseId);

            house.RenterId = null;
            await repository.SaveChangesAsync();
        }
    }
}
