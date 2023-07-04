using HouseRentingSystem.Core.Services;
using HouseRentingSystem.Core.Contracts;
using BeautySaloon.Infrastructure.Data.Common;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HouseRentingServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IHouseService, HouseService>();

            return services;
        }
    }
}
