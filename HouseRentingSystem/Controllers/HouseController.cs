using AutoMapper;
using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Extensions;
using HouseRentingSystem.Models.House;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualBasic;
using static HouseRentingSystem.Areas.Admin.AdminConstants;

namespace HouseRentingSystem.Controllers
{
    [Authorize]
    public class HouseController : Controller
    {
        private readonly IHouseService houseService;
        private readonly IAgentService agentService;
        private readonly IMapper mapper;
        private readonly IMemoryCache cache;

        public HouseController(IHouseService houseService,
            IAgentService agentService,
            IMapper mapper,
            IMemoryCache cache)
        {
            this.houseService = houseService;
            this.agentService = agentService;
            this.mapper = mapper;
            this.cache = cache;
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
            if (this.User.IsAdmin())
            {
                return RedirectToAction("Mine", "House", new { area = "Admin" });
            }

            IEnumerable<HouseServiceModel> myHouses;

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
        public async Task<IActionResult> Details(int id, string information)
        {
            if (!(await this.houseService.Exists(id)))
            {
                return BadRequest();
            }

            var houseModel = await this.houseService.HouseDetailById(id);

            if (information != houseModel.GetInformation())
            {
                return BadRequest();
            }

            return View(houseModel);
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

            return RedirectToAction(nameof(Details), new { id = newHouseId, information = model.GetInformation() });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            if (!(await this.houseService.Exists(id)))
            {
                return BadRequest();
            }

            if (!(await this.houseService.HasAgentWithId(id, this.User.Id()))
                && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var house = await this.houseService.HouseDetailById(id);

            var houseCategoryId = await this.houseService.GetHouseCategoryId(house.Id);

            var houseModel = this.mapper.Map<HouseFormModel>(house);
            houseModel.CategoryId = houseCategoryId;
            houseModel.Categories = await this.houseService.AllCategories();

            return View(houseModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, HouseFormModel model)
        {
            if (!(await this.houseService.Exists(id)))
            {
                return this.View();
            }

            if (!(await this.houseService.HasAgentWithId(id, this.User.Id()))
                && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            if (!(await this.houseService.CategoryExists(model.CategoryId)))
            {
                this.ModelState.AddModelError(nameof(model.CategoryId),
                    "Category does not exists.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await this.houseService.AllCategories();

                return View(model);
            }

            await this.houseService.Edit(id,
                model.Title,
                model.Address,
                model.Description,
                model.ImageUrl,
                model.PricePerMonth,
                model.CategoryId);

            return RedirectToAction(nameof(Details), new { id = id, information = model.GetInformation() });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            if (!(await this.houseService.Exists(id)))
            {
                return BadRequest();
            }

            if (!(await this.houseService.HasAgentWithId(id, this.User.Id())) 
                && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            var house = await this.houseService.HouseDetailById(id);

            var model = this.mapper.Map<HouseDetailsViewModel>(house);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HouseDetailsViewModel model)
        {
            if (!(await this.houseService.Exists(model.Id)))
            {
                return BadRequest();
            }

            if (!(await this.houseService.HasAgentWithId(model.Id, this.User.Id()))
                && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            await this.houseService.Delete(model.Id);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Rent(int id)
        {
            if (!(await this.houseService.Exists(id)))
            {
                return BadRequest();
            }

            if (await this.agentService.ExistsById(this.User.Id())
                && !this.User.IsAdmin())
            {
                return Unauthorized();
            }

            if (await this.houseService.IsRented(id))
            {
                return BadRequest();
            }

            await this.houseService.Rent(id, this.User.Id());

            this.cache.Remove(RentCacheKey);

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            if (!(await this.houseService.Exists(id)) ||
                !(await this.houseService.IsRented(id)))
            {
                return BadRequest();
            }

            if (!(await this.houseService.IsRentedByUserWithId(id ,this.User.Id())))
            {
                return Unauthorized();
            }

            await this.houseService.Leave(id);

            this.cache.Remove(RentCacheKey);

            return RedirectToAction(nameof(Mine));
        }
    }
}
