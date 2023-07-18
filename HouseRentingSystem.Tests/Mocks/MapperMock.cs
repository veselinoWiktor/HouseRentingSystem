using AutoMapper;
using HouseRentingSystem.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseRentingSystem.Tests.Mocks
{
    public static class MapperMock
    {
        public static IMapper Instance 
        {
            get
            {
                var mapperConfiguration = new MapperConfiguration(config =>
                {
                    config.AddProfile<ServiceMappingProfile>();
                });

                return new Mapper(mapperConfiguration);
            }
        }
    }
}
