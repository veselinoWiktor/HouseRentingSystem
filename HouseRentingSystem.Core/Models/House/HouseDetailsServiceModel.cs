using HouseRentingSystem.Core.Models.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Core.Models.House
{
    public class HouseDetailsServiceModel : HouseServiceModel
    {
        public string Description { get; init; } = null!;

        public string Category { get; init; } = null!;

        public AgentServiceModel Agent { get; init; } = null!;
    }
}
