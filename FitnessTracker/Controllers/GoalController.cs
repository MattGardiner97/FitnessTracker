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

        [NonAction]
        public async Task<FitnessUser> GetUser()
        {
            return await userManager.GetUserAsync(HttpContext.User);
        }

        [HttpGet]
        public async Task<IActionResult> Summary()
        {
            FitnessUser currentUser = await GetUser();

            Goal[] goals = await dbContext.Goals.Where(goal => goal.User == currentUser).ToArrayAsync();

            return View(goals);
        }

        [HttpGet]
        public async Task<IActionResult> AddGoal()
        {
            WeightliftingGoal model = new WeightliftingGoal()
            {
                ID = 0
            };

            return View("editgoal", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditGoal(long ID)
        {
            FitnessUser currentUser = await GetUser();

            Goal goal = await dbContext.Goals.FirstOrDefaultAsync(goal => goal.ID == ID && goal.User == currentUser);
            if (goal == null)
                return BadRequest();

            return View(goal);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteGoal(int ID)
        {
            FitnessUser currentUser = await GetUser();

            Goal goal = await dbContext.Goals.FirstOrDefaultAsync(goal => goal.ID == ID && goal.User == currentUser);
            if (goal == null)
                return BadRequest();

            dbContext.Goals.Remove(goal);
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Summary");

        }

        [HttpPost]
        public async Task<IActionResult> EditGoal(EditGoalInputModel GoalInput)
        {
            if (TryValidateModel(GoalInput) == false)
                return BadRequest();

            FitnessUser currentUser = await GetUser();

            if (GoalInput.ID != 0)
            {
                Goal existingGoal = await dbContext.Goals.FirstOrDefaultAsync(goal => goal.ID == GoalInput.ID && goal.User == currentUser);
                switch (existingGoal)
                {
                    case WeightliftingGoal wGoal:
                        wGoal.Reps = GoalInput.Reps;
                        wGoal.Weight = GoalInput.Weight;
                        break;
                    case TimedGoal tGoal:
                        tGoal.Quantity = (int)GoalInput.Quantity;
                        tGoal.QuantityUnit = GoalInput.QuantityUnit;
                        tGoal.Time = new TimeSpan(GoalInput.Hours, GoalInput.Minutes, GoalInput.Seconds);
                        break;
                }
                existingGoal.Activity = GoalInput.Activity;
                dbContext.Goals.Update(existingGoal);
            }
            else
            {
                Goal newGoal = null;
                if (GoalInput.Type.ToLower() == "weightlifting")
                {
                    newGoal = new WeightliftingGoal()
                    {
                        Weight = GoalInput.Weight,
                        Reps = GoalInput.Reps
                    };
                }
                else if (GoalInput.Type.ToLower() == "timed")
                {
                    newGoal = new TimedGoal()
                    {
                        Quantity = (int)GoalInput.Quantity,
                        QuantityUnit = GoalInput.QuantityUnit,
                        Time = new TimeSpan(GoalInput.Hours, GoalInput.Minutes, GoalInput.Seconds)
                    };
                }
                newGoal.Activity = GoalInput.Activity;
                newGoal.User = currentUser;
                dbContext.Goals.Add(newGoal);
            }
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Summary");
        }

        [HttpPost]
        public async Task<IActionResult> AddProgress(AddGoalProgressInputModel Progress)
        {
            FitnessUser currentUser = await GetUser();

            Goal goal = await dbContext.Goals.FirstOrDefaultAsync(goal => goal.ID == Progress.GoalID && goal.User == currentUser);
            if (goal == null)
                return BadRequest();

            switch (Progress.Type.ToLower())
            {
                case "weightlifting":
                    WeightliftingProgress wp = new WeightliftingProgress()
                    {
                        Date = Progress.Date,
                        Goal = goal,
                        User = currentUser,
                        Weight = Progress.Weight,
                        Reps = Progress.Reps
                    };
                    dbContext.WeightliftingProgressRecords.Add(wp);
                    break;
                case "timed":
                    TimedProgress tp = new TimedProgress()
                    {
                        Date = Progress.Date,
                        Goal = goal,
                        User = currentUser,
                        Time = new TimeSpan(Progress.Hours, Progress.Minutes, Progress.Seconds),
                        Quantity = Progress.Quantity
                    };
                    dbContext.TimedProgressRecords.Add(tp);
                    break;
                default:
                    return BadRequest();
            }

            await dbContext.SaveChangesAsync();

            return RedirectToAction("ViewGoal", new { ID = Progress.GoalID });
        }

        [HttpGet]
        public async Task<IActionResult> ViewGoal(long ID)
        {
            FitnessUser currentUser = await GetUser();

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
            FitnessUser currentUser = await GetUser();

            Goal targetGoal = await dbContext.Goals.FirstOrDefaultAsync(goal => goal.ID == GoalID && goal.User == currentUser);
            if (targetGoal == null)
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
            FitnessUser currentUser = await GetUser();

            Goal targetGoal = await dbContext.Goals.FirstOrDefaultAsync(Goal => Goal.ID == GoalID && Goal.User == currentUser);
            if (targetGoal == null)
                return BadRequest();

            var progress = await dbContext.TimedProgressRecords
                .Where(record => record.Goal.ID == GoalID)
                .OrderBy(record => record.Date)
                .Select(record => new { Date = record.Date.ToString("d"), Timespan = record.Time, Quantity = record.Quantity, QuantityUnit=record.QuantityUnit})
                .ToArrayAsync();

            return Json(progress);
        }
    }
}
