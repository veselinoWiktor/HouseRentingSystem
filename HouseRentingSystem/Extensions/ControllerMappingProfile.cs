using AutoMapper;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Models.House;

namespace HouseRentingSystem.Extensions
{
    public class ControllerMappingProfile : Profile
    {
        public ControllerMappingProfile()
        {
            this.CreateMap<HouseDetailsServiceModel, HouseFormModel>();
            this.CreateMap<HouseDetailsServiceModel, HouseDetailsViewModel>();
        }
    }
}
