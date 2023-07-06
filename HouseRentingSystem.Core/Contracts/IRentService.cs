using HouseRentingSystem.Core.Models.Rent;

namespace HouseRentingSystem.Core.Contracts
{
    public interface IRentService
    {
        Task<IEnumerable<RentServiceModel>> All();
    }
}
