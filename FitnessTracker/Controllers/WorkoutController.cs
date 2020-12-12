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
    public class WorkoutController : Controller
    {
        private ApplicationDbContext dbContext;
        private UserManager<FitnessUser> userManager;

        public WorkoutController(ApplicationDbContext DBContext, UserManager<FitnessUser> UserManager)
        {
            dbContext = DBContext;
            userManager = UserManager;
        }

        [NonAction]
        public async Task<FitnessUser> GetUser()
        {
            return await userManager.GetUserAsync(User);
        }

        public IActionResult Summary()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            WorkoutPlan newPlan = new WorkoutPlan()
            {
                Name = "Workout Plan"
            };
            return View("/Views/Workout/edit.cshtml", newPlan);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long ID)
        {
            WorkoutPlan plan = await dbContext.WorkoutPlans.FirstOrDefaultAsync(plan => plan.ID == ID);

            return View(plan);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WorkoutPlan WorkoutPlan)
        {
            FitnessUser currentUser = await GetUser();
            WorkoutPlan.User = currentUser;

            if (WorkoutPlan.ID == 0)
                dbContext.WorkoutPlans.Add(WorkoutPlan);
            else
                dbContext.WorkoutPlans.Update(WorkoutPlan);

            await dbContext.SaveChangesAsync();

            return RedirectToAction("Summary");
        }
    }
}
