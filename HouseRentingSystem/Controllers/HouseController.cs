using HouseRentingSystem.Core.Contracts;
using HouseRentingSystem.Core.Models.House;
using HouseRentingSystem.Extensions;
using HouseRentingSystem.Infrastructure.Data.Entities;
using HouseRentingSystem.Models.House;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HouseRentingSystem.Controllers
{
    [Authorize]
    public class HouseController : Controller
    {
        private readonly IHouseService houseService;
        private readonly IAgentService agentService;

        public HouseController(IHouseService _houseService, IAgentService _agentService)
        {
            houseService = _houseService;
            agentService = _agentService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> All([FromQuery] AllHousesQueryModel query)
        {
            var queryResult = await houseService.All(
                query.Category,
                query.SearchTerm,
                query.Sorting,
                query.CurrentPage,
                AllHousesQueryModel.HousesPerPage);

            query.TotalHousesCount = queryResult.TotalHousesCount;
            query.Houses = queryResult.Houses;

            var houseCategories = await houseService.AllCategoriesNames();
            query.Categories = houseCategories;

            return View(query);
        }

        public async Task<IActionResult> Mine()
        {
            IEnumerable<HouseServiceModel> myHouses = null;

            var userId = User.Id();

            if (await agentService.ExistsById(userId))
            {
                var currentAgentId = await agentService.GetAgentId(userId);

                myHouses = await houseService.AllHousesByAgentId(currentAgentId);
            }
            else
            {
                myHouses = await houseService.AllHousesByUserId(userId);
            }

            return View(myHouses);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            if (!(await houseService.Exists(id)))
            {
                return BadRequest();
            }

            var houseModel = await houseService.HouseDetailsById(id);

            return View(houseModel);
        }

        public async Task<IActionResult> Add()
        {
            if (!(await agentService.ExistsById(User.Id())))
            {
                return RedirectToAction(nameof(AgentController.Become), "Agent");
            }

            return View(new HouseFormModel
            {
                Categories = await houseService.AllCategories()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add(HouseFormModel model)
        {
            if (!(await agentService.ExistsById(User.Id())))
            {
                return RedirectToAction(nameof(AgentController.Become), "Agent");
            }

            if (!(await houseService.CategoryExists(model.CategoryId)))
            {
                ModelState.AddModelError(nameof(model.CategoryId),
                    "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await houseService.AllCategories();

                return View(model);
            }

            var agentId = await agentService.GetAgentId(User.Id());

            var newHouseId = await houseService.Create(model.Title,
                model.Address, model.Description,
                model.ImageUrl, model.PricePerMonth,
                model.CategoryId, agentId);

            return RedirectToAction(nameof(Details), new { id = newHouseId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!(await houseService.Exists(id)))
            {
                return BadRequest();
            }
            if (!(await houseService.HasAgentWithId(id, User.Id())))
            {
                return Unauthorized();
            }

            var house = await houseService.HouseDetailsById(id);

            var houseCategoryId = await houseService.GetHouseCategoryId(house.Id);

            var houseModel = new HouseFormModel()
            {
                Title = house.Title,
                Address = house.Address,
                Description = house.Description,
                ImageUrl = house.ImageUrl,
                PricePerMonth = house.PricePerMonth,
                CategoryId = houseCategoryId,
                Categories = await houseService.AllCategories()
            };

            return View(houseModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, HouseFormModel model)
        {
            if (!(await houseService.Exists(id)))
            {
                return View();
            }

            if (!(await houseService.HasAgentWithId(id, User.Id())))
            {
                return Unauthorized();
            }

            if (!(await houseService.CategoryExists(model.CategoryId)))
            {
                ModelState.AddModelError(nameof(model.CategoryId),
                    "Category does not exist.");
            }

            if (!ModelState.IsValid)
            {
                model.Categories = await houseService.AllCategories();

                return View();
            }

            await houseService.Edit(id, model.Title, model.Address, model.Description, model.ImageUrl, model.PricePerMonth, model.CategoryId);

            return RedirectToAction(nameof(Details), new { id = id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!(await houseService.Exists(id)))
            {
                return BadRequest();
            }

            if (!(await houseService.HasAgentWithId(id, User.Id())))
            {
                return Unauthorized();
            }

            var house = await houseService.HouseDetailsById(id);

            var model = new HouseDetailsViewModel()
            {
                Title = house.Title,
                Address = house.Address,
                ImageUrl = house.ImageUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HouseDetailsViewModel model)
        {
            if (!(await houseService.Exists(model.Id)))
            {
                return BadRequest();
            }

            if (!(await houseService.HasAgentWithId(model.Id, User.Id())))
            {
                return Unauthorized();
            }

            await houseService.Delete(model.Id);

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Rent(int id)
        {
            if (!(await houseService.Exists(id)))
            {
                return BadRequest();
            }

            if (await agentService.ExistsById(User.Id()))
            {
                return Unauthorized();
            }

            if (await houseService.IsRented(id))
            {
                return BadRequest();
            }

            if (await houseService.IsRentedByUserWithId(id, User.Id()))
            {
                return BadRequest();
            }

            await houseService.Rent(id, User.Id());

            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public async Task<IActionResult> Leave(int id)
        {
            if (!(await houseService.Exists(id)) ||
                !(await houseService.IsRented(id)))
            {
                return BadRequest();
            }

            if (!(await houseService.IsRentedByUserWithId(id, User.Id())))
            {
                return Unauthorized();
            }

            await houseService.Leave(id);

            return RedirectToAction(nameof(Mine));
        }
    }
}
