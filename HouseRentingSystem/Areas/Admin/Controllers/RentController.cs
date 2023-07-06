using HouseRentingSystem.Core.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Areas.Admin.Controllers
{
    public class RentController : AdminController
    {
        private readonly IRentService rentService;

        public RentController(IRentService rentService)
        {
            this.rentService = rentService;
        }

        [Route("Rent/All")]
        public async Task<IActionResult> All()
        {
            var rents = await this.rentService.All();
            return View(rents);
        }
    }
}
