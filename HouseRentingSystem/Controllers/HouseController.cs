using HouseRentingSystem.Core.Models.House;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    public class HouseController : Controller
    {
        
        public async Task<IActionResult> All()
        {
            return View(new AllHousesQueryModel());
        }
    }
}
