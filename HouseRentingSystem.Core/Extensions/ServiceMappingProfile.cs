using AutoMapper;
using HouseRentingSystem.Core.Models.Agent;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Core.Models.Rent;
using HouseRentingSystem.Core.Models.User;
using HouseRentingSystem.Infrastructure.Data.Entities;

namespace HouseRentingSystem.Core.Extensions
{
    public class ServiceMappingProfile : Profile
    {
        public ServiceMappingProfile()
        {
            this.CreateMap<House, HouseServiceModel>()
                .ForMember(h => h.IsRented, cfg => cfg.MapFrom(h => h.RenterId != null));

            this.CreateMap<House, HouseDetailsServiceModel>()
                .ForMember(h => h.IsRented, cfg => cfg.MapFrom(h => h.RenterId != null))
                .ForMember(h => h.Category, cfg => cfg.MapFrom(h => h.Category.Name));

            this.CreateMap<Agent, AgentServiceModel>()
                .ForMember(a => a.Email, cfg => cfg.MapFrom(a => a.User.Email));

            this.CreateMap<House, HouseIndexServiceModel>();

            this.CreateMap<Category, HouseCategorySeviceModel>();

            this.CreateMap<Agent, UserServiceModel>()
                .ForMember(u => u.Email, cfg => cfg.MapFrom(a => a.User.Email))
                .ForMember(u => u.FullName, cfg => cfg.MapFrom(a => a.User.FirstName + " " + a.User.LastName));

            this.CreateMap<User, UserServiceModel>()
                .ForMember(u => u.PhoneNumber, cfg => cfg.MapFrom(u => String.Empty))
                .ForMember(u => u.FullName, cfg => cfg.MapFrom(u => u.FirstName + " " + u.LastName));

            this.CreateMap<House, RentServiceModel>()
                .ForMember(h => h.HouseTitle, cfg => cfg.MapFrom(h => h.Title))
                .ForMember(h => h.HouseImageUrl, cfg => cfg.MapFrom(h => h.ImageUrl))
                .ForMember(h => h.AgentFullName, cfg => cfg.MapFrom(h => h.Agent.User.FirstName + " " + h.Agent.User.LastName))
                .ForMember(h => h.AgentEmail, cfg => cfg.MapFrom(h => h.Agent.User.Email))
                .ForMember(h => h.RenterFullName, cfg => cfg.MapFrom(h => h.Renter.FirstName + " " + h.Renter.LastName))
                .ForMember(h => h.RenterEmail, cfg => cfg.MapFrom(h => h.Renter.Email));
        }
    }
}
