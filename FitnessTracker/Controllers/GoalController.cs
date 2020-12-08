using FitnessTracker.Data;
using FitnessTracker.Models;
using FitnessTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);
            WeightliftingGoal model = new WeightliftingGoal()
            {
                ID = 0,
                User = currentUser
            };


            return View("editgoal", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditGoal(int ID)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            Goal goal = await dbContext.Goals.FirstOrDefaultAsync(goal => goal.ID == ID && goal.User == currentUser);
            if (goal == null)
                return BadRequest();

            return View(goal);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteGoal(int ID)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            Goal goal = await dbContext.Goals.FirstOrDefaultAsync(goal => goal.ID == ID && goal.User == currentUser);
            if (goal == null)
                return BadRequest();

            dbContext.Goals.Remove(goal);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Summary");

        }

        [HttpPost]
        public async Task<IActionResult> EditGoal(int ID, string Activity, string Type, float Weight, int Reps, int Hours, int Minutes, int Seconds, float Quantity, string QuantityUnit)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            Goal existingGoal = await dbContext.Goals.FirstOrDefaultAsync(goal => goal.ID == ID && goal.User == currentUser);
            if (existingGoal != null)
                dbContext.Goals.Remove(existingGoal); //We remove the entry in case the Goal subtype has been changed.
            else
            {
                if (ID != 0)
                    return BadRequest(); //This means a client is trying to edit a goal which either doesn't exist or doesn't belong to them.
            }

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

        [HttpPost]
        public async Task<IActionResult> AddProgress(int GoalID, string Type, DateTime Date, float Weight, int Reps, int Quantity, int Hours, int Minutes, int Seconds)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            Goal goal = await dbContext.Goals.FirstOrDefaultAsync(goal => goal.ID == GoalID && goal.User == currentUser);
            if (goal == null)
                return BadRequest();

            switch (Type.ToLower())
            {
                case "weightlifting":
                    WeightliftingProgress wp = new WeightliftingProgress()
                    {
                        Date = Date,
                        Goal = goal,
                        User = currentUser,
                        Weight = Weight,
                        Reps = Reps
                    };
                    dbContext.WeightliftingProgressRecords.Add(wp);
                    break;
                case "timed":
                    TimedProgress tp = new TimedProgress()
                    {
                        Date = Date,
                        Goal = goal,
                        User = currentUser,
                        Time = new TimeSpan(Hours, Minutes, Seconds),
                        Quantity = Quantity
                    };
                    dbContext.TimedProgressRecords.Add(tp);
                    break;
                default:
                    return BadRequest();
            }

            await dbContext.SaveChangesAsync();

            return RedirectToAction("ViewGoal", new { ID = GoalID });
        }

        [HttpGet]
        public async Task<IActionResult> ViewGoal(long ID)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            Goal goal = await dbContext.Goals.FirstOrDefaultAsync(goal => goal.ID == ID && goal.User == currentUser);
            if (goal == null)
                return BadRequest();

            GoalProgress[] progress = await dbContext
                .GoalProgressRecords.Where(record => record.Goal == goal && record.User == currentUser)
                .OrderByDescending(progress => progress.Date)
                .ToArrayAsync();

            if (progress == null)
                return BadRequest();

            GoalViewModel viewModel = new GoalViewModel()
            {
                Goal = goal,
                Progress = progress
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetWeightliftingProgress(long GoalID)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            Goal targetGoal = await dbContext.Goals.FirstOrDefaultAsync(goal => goal.ID == GoalID);
            if (targetGoal.User != currentUser)
                return BadRequest();

            var progress = await dbContext.WeightliftingProgressRecords
                .Where(record => record.Goal.ID == GoalID)
                .OrderBy(record => record.Date)
                .Select(record => new { Date = record.Date.ToString("d"), Weight = record.Weight, Reps = record.Reps })
                .ToArrayAsync();

            return Json(progress);
        }

        [HttpGet]
        public async Task<IActionResult> GetTimedProgress(long GoalID)
        {
            return BadRequest();
        }
    }
}
