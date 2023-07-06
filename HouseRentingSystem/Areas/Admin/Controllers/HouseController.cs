using HouseRentingSystem.Areas.Admin.Models;
using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Areas.Admin.Controllers
{
    public class HouseController : AdminController
    {
        private readonly IHouseService houseService;
        private readonly IAgentService agentService;

        public HouseController(IHouseService houseService, IAgentService agentService)
        {
            this.houseService = houseService;
            this.agentService = agentService;
        }

        public async Task<IActionResult> Mine()
        {
            var myHouses = new MyHousesViewModel();

            var adminUserId = this.User.Id();
            myHouses.RentedHouses = await this.houseService.AllHousesByUserId(adminUserId);

            var adminAgentId = await this.agentService.GetAgentId(adminUserId);
            myHouses.AddedHouses = await this.houseService.AllHousesByAgentId(adminAgentId);

            return View(myHouses);
        }
    }
}
