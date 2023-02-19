using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Core.Models.House
{
    public class HouseIndexServiceModel
    {
        public int Id { get; init; }

        public string Title { get; init; } = null!;

        public string ImageUrl { get; init; } = null!;
    }
}
