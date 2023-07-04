using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Extensions;
using HouseRentingSystem.Models.Agent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    [Authorize]
    public class AgentController : Controller
    {
        private readonly IAgentService agentService;

        public AgentController(IAgentService agentService)
        {
            this.agentService = agentService;
        }

        public async Task<IActionResult> Become()
        {
            if (await this.agentService.ExistsById(this.User.Id()))
            {
                return BadRequest();
            }

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Become(BecomeAgentFormModel model)
        {
            var userId = this.User.Id();
            if (await this.agentService.ExistsById(userId))
            {
                return BadRequest();
            }

            if (await this.agentService.UserWithPhoneNumberExists(model.PhoneNumber))
            {
                ModelState.AddModelError(nameof(model.PhoneNumber),
                    "Phone number alredy exists. Enter another one.");
            }

            if (await this.agentService.UserHasRents(userId))
            {
                ModelState.AddModelError("Error",
                    "You should have no rent to become agent!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.agentService.Create(userId, model.PhoneNumber);

            return RedirectToAction(nameof(HouseController.All), "House"); 
        }
    }
}
