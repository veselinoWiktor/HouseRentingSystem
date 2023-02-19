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

        public AgentController(IAgentService _agentService)
        {
            agentService = _agentService;
        }

        public async Task<IActionResult> Become()
        {
            if (await agentService.ExistsById(User.Id()))
            {
                return BadRequest();
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Become(BecomeAgentFormModel model)
        {
            var userId = User.Id();

            if (await agentService.ExistsById(userId))
            {
                return BadRequest();
            }

            if (await agentService.UserWithPhoneNumberExists(model.PhoneNumber))
            {
                ModelState.AddModelError(nameof(model.PhoneNumber),
                    "Phone number already exists. Enter another one.");
            }

            if (await agentService.UserHasRents(userId))
            {
                ModelState.AddModelError("Error",
                    "You should have no rents to become agent!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await agentService.Create(userId, model.PhoneNumber);

            return RedirectToAction(nameof(HouseController.All), "House");
        }
    }
}
