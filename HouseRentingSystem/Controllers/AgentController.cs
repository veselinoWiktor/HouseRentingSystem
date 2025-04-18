﻿using HouseRentingSystem.Core.Contracts;
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
        private readonly IUserService userService;

        public AgentController(IAgentService agentService, IUserService userService)
        {
            this.agentService = agentService;
            this.userService = userService;
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

            if (await this.agentService.AgentWithPhoneNumberExists(model.PhoneNumber))
            {
                ModelState.AddModelError(nameof(model.PhoneNumber),
                    "Phone number alredy exists. Enter another one.");
            }

            if (await this.userService.UserHasRents(userId))
            {
                ModelState.AddModelError("Error",
                    "You should have no rent to become agent!");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await this.agentService.Create(userId, model.PhoneNumber);

            TempData["message"] = "You have successfully become an agent";

            return RedirectToAction(nameof(HouseController.All), "House"); 
        }
    }
}
