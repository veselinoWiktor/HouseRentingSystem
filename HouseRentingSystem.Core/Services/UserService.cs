using AutoMapper;
using AutoMapper.QueryableExtensions;
using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.User;
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
    public class UserService : IUserService
    {
        private readonly IRepository repo;
        private readonly IMapper mapper;

        public UserService(IRepository repo, IMapper mapper)
        {
            this.repo = repo;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<UserServiceModel>> All()
        {
            var allUsers = new List<UserServiceModel>();

            var agents = await this.repo.AllReadonly<Agent>()
                .Include(a => a.User)
                .ProjectTo<UserServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            allUsers.AddRange(agents);

            var users = await this.repo.AllReadonly<User>()
                .Where(u => !this.repo.AllReadonly<Agent>().Any(ag => ag.UserId == u.Id))
                .ProjectTo<UserServiceModel>(this.mapper.ConfigurationProvider)
                .ToListAsync();

            allUsers.AddRange(users);

            return allUsers;
        }

        public async Task<string> UserFullName(string userId)
        {
            var user = await this.repo.GetByIdAsync<User>(userId);

            if (String.IsNullOrEmpty(user.FirstName)
                || String.IsNullOrEmpty(user.LastName))
            {
                return null;
            }

            return user.FirstName + " " + user.LastName;
        }
    }
}
