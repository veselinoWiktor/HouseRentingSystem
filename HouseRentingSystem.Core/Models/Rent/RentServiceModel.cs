using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Core.Models.Rent
{
    public class RentServiceModel
    {
        public string HouseTitle { get; init; } = null!;

        public string HouseImageUrl { get; init; } = null!;

        public string AgentFullName { get; init; } = null!;

        public string AgentEmail { get; init; } = null!;

        public string RenterFullName { get; init; } = null!;

        public string RenterEmail { get; init; } = null!;
    }
}
