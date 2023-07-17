using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Core.Contracts
{
    public interface IAgentService
    {
        Task<bool> ExistsById(string userId);

        Task<bool> AgentWithPhoneNumberExists(string phoneNumber);


        Task Create(string userId, string phoneNumber);

        Task<int> GetAgentId(string userId);
    }
}
