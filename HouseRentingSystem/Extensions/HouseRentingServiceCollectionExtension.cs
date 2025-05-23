﻿using HouseRentingSystem.Core.Services;
using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Infrastructure.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HouseRentingServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IHouseService, HouseService>();
            services.AddScoped<IAgentService, AgentService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRentService, RentService>();
            services.AddScoped<IStatisticsService, StatisticsService>();

            return services;
        }
    }
}
