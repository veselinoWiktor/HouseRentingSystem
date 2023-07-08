using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.Rent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static HouseRentingSystem.Areas.Admin.AdminConstants;

namespace HouseRentingSystem.Areas.Admin.Controllers
{
    public class RentController : AdminController
    {
        private readonly IRentService rentService;
        private readonly IMemoryCache cache;

        public RentController(IRentService rentService,
            IMemoryCache cache)
        {
            this.rentService = rentService;
            this.cache = cache;
        }

        [Route("Rent/All")]
        public async Task<IActionResult> All()
        {
            var rents = this.cache.Get<IEnumerable<RentServiceModel>>(RentCacheKey);

            if (rents == null)
            {
                rents = await this.rentService.All();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                this.cache.Set(RentCacheKey, rents, cacheOptions);
            }
            
            return View(rents);
        }
    }
}
