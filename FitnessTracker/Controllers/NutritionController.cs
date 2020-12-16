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
    public class NutritionSummaryModel
    {
        public FoodRecord[] Records { get; set; }
        public NutritionTarget Target { get; set; }
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
            if (FoodIDs.Length != Quantities.Length || FoodIDs.Length == 0)
                return BadRequest();

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

        [HttpGet]
        public async Task<IActionResult> Summary()
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            FoodRecord[] userRecords = await dbContext.FoodRecords
                .Where(record => record.User == currentUser && record.ConsumptionDate >= DateTime.Today.AddDays(-28))
                .Include(record => record.Food)
                .ToArrayAsync();
            NutritionTarget userTarget = await dbContext.NutritionTargets.FirstOrDefaultAsync(record => record.User == currentUser);
            if (userTarget == null)
                userTarget = new NutritionTarget();

            NutritionSummaryModel summaryModel = new NutritionSummaryModel()
            {
                Records = userRecords,
                Target = userTarget
            };

            return View(summaryModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetNutritionData(uint PreviousDays = 7)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);
            var records = await dbContext.FoodRecords
                .Where(record => record.ConsumptionDate >= DateTime.Today.AddDays(-PreviousDays) && record.User == currentUser)
                .Include(record => record.Food)
                .ToArrayAsync();

                var result = records
                .GroupBy(record => record.ConsumptionDate)
                .Select(grouping =>
                new
                {
                    Date = grouping.Key.ToString("d"),
                    Calories = grouping.Sum(r => r.Food.Calories),
                    Carbs = grouping.Sum(r => r.Food.Carbohydrates),
                    Protein = grouping.Sum(r => r.Food.Protein),
                    Fat = grouping.Sum(r => r.Food.Fat)
                })
                .ToArray();

            return Json(result);
        }
    }


}
