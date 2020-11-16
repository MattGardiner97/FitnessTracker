using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Controllers
{
    public class NewFoodModel
    {
        public Food[] UserFoods { get; set; }
        public FoodRecord[] FoodRecords { get; set; }
    }

    [Authorize]
    public class NutritionController : Controller
    {
        private ApplicationDbContext dbContext;
        private UserManager<FitnessUser> userManager;

        public NutritionController(ApplicationDbContext DBContext, UserManager<FitnessUser> UserManager)
        {
            dbContext = DBContext;
            userManager = UserManager;
        }

        [HttpGet]
        public async Task<IActionResult> AddFood(DateTime Date)
        {
            if (Date.Ticks == 0)
                Date = DateTime.Today;
            ViewData["selectedDate"] = Date;

            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            NewFoodModel model = new NewFoodModel()
            {
                FoodRecords = await dbContext.FoodRecords.Where(record => record.User == currentUser && record.ConsumptionDate == Date).ToArrayAsync(),
                UserFoods = await dbContext.UserFoods.Where(record => record.CreatedBy == currentUser).ToArrayAsync()
            };



            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewFood(Food Food)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);
            Food.CreatedBy = currentUser;

            if (Food.ID == 0)
                dbContext.UserFoods.Add(Food);
            else
                dbContext.UserFoods.Update(Food);

            await dbContext.SaveChangesAsync();

            return RedirectToAction("AddFood");
        }

        [HttpPost]
        public async Task<IActionResult> EditRecords(DateTime Date, long[] FoodIDs, float[] Quantities)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            FoodRecord[] existingRecords = await dbContext.FoodRecords.Where(record => record.User == currentUser && record.ConsumptionDate == Date).ToArrayAsync();
            dbContext.FoodRecords.RemoveRange(existingRecords);

            FoodRecord[] newRecords = new FoodRecord[FoodIDs.Length];
            for (int i = 0; i < FoodIDs.Length; i++)
            {
                newRecords[i] = new FoodRecord()
                {
                    ConsumptionDate = Date,
                    User = currentUser,
                    FoodID = FoodIDs[i],
                    Quantity = Quantities[i]
                };
            }
            dbContext.FoodRecords.AddRange(newRecords);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("AddFood");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFood(long ID)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            Food targetFood = await dbContext.UserFoods.FirstOrDefaultAsync(food => food.ID == ID);
            if (targetFood == null || targetFood.CreatedBy != currentUser)
                return BadRequest();

            dbContext.UserFoods.Remove(targetFood);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("AddFood");
        }
    }
}
