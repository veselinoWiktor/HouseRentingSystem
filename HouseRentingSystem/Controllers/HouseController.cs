﻿using HouseRentingSystem.Core.Models.House;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HouseRentingSystem.Controllers
{
    [Authorize]
    public class HouseController : Controller
    {
        [AllowAnonymous]
        public IActionResult All()
        {
            return View(new AllHousesQueryModel());
        }

        public IActionResult Mine()
        {
            return View(new AllHousesQueryModel());
        }

        public IActionResult Details(int id)
        {
            return View(new HouseDetailsViewModel());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(HouseFormModel model)
        {
            return RedirectToAction(nameof(Details), new { id = 1 });
        }

        public IActionResult Edit(int id)
        {
            return View(new HouseDetailsViewModel());
        }

        [HttpPost]
        public IActionResult Edit(int id, HouseFormModel model)
        {
            return RedirectToAction(nameof(Details), new { id = "1" });
        }

        public IActionResult Delete(int id)
        {
            return View(new HouseDetailsViewModel());
        }

        [HttpPost]
        public IActionResult Delete(HouseDetailsViewModel model)
        {
            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public IActionResult Rent(int id)
        {
            return RedirectToAction(nameof(Mine));
        }

        [HttpPost]
        public IActionResult Leave(int id)
        {
            return RedirectToAction(nameof(Mine));
        }
    }
}