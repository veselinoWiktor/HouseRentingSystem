using HouseRentingSystem.Core.Models;
using HouseRentingSystem.Core.Models.House;

namespace HouseRentingSystem.Core.Contracts
{
    public interface IHouseService
    {
        Task<IEnumerable<HouseIndexServiceModel>> LastThreeHouses();

        Task<IEnumerable<HouseCategorySeviceModel>> AllCategories();

        Task<bool> CategoryExists(int categoryId);

        Task<int> Create(
            string title,
            string address,
            string description,
            string imageUrl,
            decimal price,
            int categoryId,
            int agentId);

        Task<HouseQueryServiceModel> All(
            string? category = null,
            string? searchTerm = null,
            HouseSorting sorting = HouseSorting.Newest,
            int currentPage = 1,
            int housesPerPage = 1);

        Task<IEnumerable<string>> AllCategoriesNames();
    }
}
