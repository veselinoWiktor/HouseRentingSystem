using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static HouseRentingSystem.Areas.Admin.AdminConstants;

namespace HouseRentingSystem.Areas.Admin.Controllers
{
    public class UserController : AdminController
    {
        private readonly IUserService userService;
        private readonly IMemoryCache cache;  

        public UserController(IUserService userService, IMemoryCache cache)
        {
            this.userService = userService;
            this.cache = cache;
        }

        [Route("User/All")]
        public async Task<IActionResult> All()
        {
            var users = this.cache
                .Get<IEnumerable<UserServiceModel>>(UserCacheKey);

            if (users == null)
            {
                users = await this.userService.All();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

                this.cache.Set(UserCacheKey, users, cacheOptions);
            }
            return View(users);
        }
    }
}
