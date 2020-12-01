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
    [Authorize]
    public class GoalController : Controller
    {
        private ApplicationDbContext dbContext;
        private UserManager<FitnessUser> userManager;

        public GoalController(ApplicationDbContext DBContext, UserManager<FitnessUser> UserManager)
        {
            dbContext = DBContext;
            userManager = UserManager;
        }

        [HttpGet]
        public async Task<IActionResult> Summary()
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            Goal[] goals = await dbContext.Goals.Where(goal => goal.User == currentUser).ToArrayAsync();

            return View(goals);
        }

        [HttpGet]
        public async Task<IActionResult> AddGoal()
        {
            return View("editgoal");
        }

        [HttpGet]
        public async Task<IActionResult> EditGoal(int ID)
        {
            Goal goal = await dbContext.Goals.FirstOrDefaultAsync(goal => goal.ID == ID);
            if (goal == null)
                return BadRequest();

            return View(goal);
        }

        [HttpPost]
        public async Task<IActionResult> EditGoal(int ID, string Activity, string Type, float Weight, int Reps, int Hours, int Minutes, int Seconds, float Quantity, string QuantityUnit)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            Goal existingGoal = await dbContext.Goals.FirstOrDefaultAsync(goal => goal.ID == ID);
            if (existingGoal != null)
                dbContext.Goals.Remove(existingGoal);

            Goal goal = null;
            if (Type == "Weightlifting")
                goal = new WeightliftingGoal()
                {
                    Reps = Reps,
                    Weight = Weight
                };
            else if (Type == "Timed")
                goal = new TimedGoal()
                {
                    Quantity = (int)Quantity,
                    QuantityUnit = QuantityUnit,
                    Time = new TimeSpan(Hours, Minutes, Seconds)
                };
            else
                return BadRequest();

            goal.Activity = Activity;
            goal.User = currentUser;

            dbContext.Goals.Add(goal);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Summary");
        }

    }
}
