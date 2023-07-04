using HouseRentingSystem.Core.Models.House;

namespace HouseRentingSystem.Core.Contracts
{
    public interface IHouseService
    {
        IEnumerable<HouseIndexServiceModel> LastThreeHouses();
    }
}
