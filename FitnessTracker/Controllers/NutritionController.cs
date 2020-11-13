using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Authorization;
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

        public NutritionController(ApplicationDbContext DBContext,UserManager<FitnessUser> UserManager)
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
        public async Task<IActionResult> AddNewFood(string Name, int ServingSize, ServingUnit ServingUnit, int Calories, int Carbs, int Protein, int Fat)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            Food newFood = new Food()
            {
                CreatedBy = currentUser,
                Name = Name,
                ServingSize = ServingSize,
                ServingUnit = ServingUnit,
                Calories = Calories,
                Carbohydrates = Carbs,
                Protein = Protein,
                Fat = Fat
            };
            dbContext.UserFoods.Add(newFood);

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
            for(int i = 0; i < FoodIDs.Length;i++)
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
    }
}
