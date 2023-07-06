using AutoMapper;
using AutoMapper.QueryableExtensions;
using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.Rent;
using HouseRentingSystem.Infrastructure.Common;
using HouseRentingSystem.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Core.Services
{
    public class RentService : IRentService
    {
        private readonly IRepository repo;
        private readonly IMapper mapper;

        public RentService(IRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<RentServiceModel>> All()
        {
            return await this.repo.AllReadonly<House>()
                .Include(h => h.Agent.User)
                .Include(h => h.Renter)
                .Where(h => h.RenterId != null)
                .ProjectTo<RentServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

        }
    }
}
