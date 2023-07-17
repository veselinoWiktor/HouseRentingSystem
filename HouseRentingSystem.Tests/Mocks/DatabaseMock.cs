using HouseRentingSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Tests.Mocks
{
    public static class DatabaseMock
    {
        public static HouseRentingDbContext Instance 
        { 
            get
            {
                var dbContextOptions = new DbContextOptionsBuilder<HouseRentingDbContext>()
                    .UseInMemoryDatabase("HouseRentingInMemoryDb"
                        + DateTime.Now.Ticks.ToString())
                    .Options;

                return new HouseRentingDbContext(dbContextOptions, false);
            }
        }
    }
}
