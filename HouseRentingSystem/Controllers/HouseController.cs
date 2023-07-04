using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Extensions;
using HouseRentingSystem.Models.House;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    [Authorize]
    public class HouseController : Controller
    {
        private readonly IHouseService houseService;
        private readonly IAgentService agentService;

        public HouseController(IHouseService houseService, IAgentService agentService)
        {
            this.houseService = houseService;
            this.agentService = agentService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AllHousesQueryModel query)
        {
            var queryResult = await this.houseService.All(
                query.Category,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllHousesQueryModel.HousesPerPage);

            query.TotalHousesCount = queryResult.TotalHousesCount;
            query.Houses = queryResult.Houses;

            var houseCategories = await this.houseService.AllCategoriesNames();
            query.Categories = houseCategories;

            return View(query);
        }

        [HttpGet]
        public async Task<IActionResult> Mine()
        {
            IEnumerable<HouseServiceModel> myHouses = null;

            var userId = this.User.Id();

            if (await this.agentService.ExistsById(userId))
            {
                var currentAgentId = await this.agentService.GetAgentId(userId);

                myHouses = await this.houseService.AllHousesByAgentId(currentAgentId);
            }
            else
            {
                myHouses = await this.houseService.AllHousesByUserId(userId);
            }

            return View(myHouses);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            return View(new HouseDetailsViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            if (!(await this.agentService.ExistsById(User.Id())))
            {
                return RedirectToAction(nameof(AgentController.Become), "Agents");
            }

            return View(new HouseFormModel()
            {
                Categories = await this.houseService.AllCategories()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(HouseFormModel model)
        {
            if (!(await this.agentService.ExistsById(this.User.Id())))
            {
                return RedirectToAction(nameof(AgentController.Become), "Agent");
            }

            if (!(await this.houseService.CategoryExists(model.CategoryId)))
            {
                this.ModelState.AddModelError(nameof(model.CategoryId),
                    "Category does not exist.");
            }

            if (!this.ModelState.IsValid)
            {
                model.Categories = await this.houseService.AllCategories();

                return View(model);
            }

            var agentId = await this.agentService.GetAgentId(this.User.Id());

            var newHouseId = await houseService.Create(model.Title,
                model.Address,
                model.Description,
                model.ImageUrl,
                model.PricePerMonth,
                model.CategoryId,
                agentId);

            return RedirectToAction(nameof(Details), new { id = newHouseId });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            return View(new HouseFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, HouseFormModel house)
        {
            return RedirectToAction(nameof(Details), new { id = "1" });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            return View(new HouseDetailsViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HouseDetailsViewModel house)
        {
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Rent(int id)
        {
            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            return RedirectToAction(nameof(Mine));
        }
    }
}
