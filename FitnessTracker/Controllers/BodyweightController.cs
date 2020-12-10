using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessTracker.Data;
using FitnessTracker.Models;
using FitnessTracker.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Controllers
{
    [Authorize]
    public class BodyweightController : Controller
    {
        private ApplicationDbContext dbContext;
        private UserManager<FitnessUser> userManager;

        public BodyweightController(ApplicationDbContext DBContext, UserManager<FitnessUser> UserManager)
        {
            dbContext = DBContext;
            userManager = UserManager;
        }

        public async Task<IActionResult> Summary()
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            IEnumerable<BodyweightRecord> records = await dbContext.BodyweightRecords
                .Where(record => record.User == currentUser)
                .OrderByDescending(record => record.Date)
                .ToArrayAsync();
            BodyweightTarget target = await dbContext.BodyweightTargets.FirstOrDefaultAsync(target => target.User == currentUser);

            BodyweightSummaryViewModel viewModel = new BodyweightSummaryViewModel(records, target);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditTarget()
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            BodyweightTarget target = await dbContext.BodyweightTargets.Where(target => target.User == currentUser).FirstOrDefaultAsync();

            return View(target);
        }

        [HttpPost]
        public async Task<IActionResult> EditTarget(float TargetWeight, DateTime TargetDate)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);


            BodyweightTarget newTarget = await dbContext.BodyweightTargets.Where(target => target.User == currentUser).FirstOrDefaultAsync();
            if (newTarget == null)
            {
                newTarget = new BodyweightTarget()
                {
                    User = currentUser,
                    TargetWeight = TargetWeight,
                    TargetDate = TargetDate
                };
                dbContext.BodyweightTargets.Add(newTarget);
            }
            else
            {
                newTarget.TargetWeight = TargetWeight;
                newTarget.TargetDate = TargetDate;
                dbContext.BodyweightTargets.Update(newTarget);
            }
            await dbContext.SaveChangesAsync();

            return RedirectToAction("Summary");
        }

        [HttpGet]
        public async Task<IActionResult> EditRecords()
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);



            BodyweightRecord[] records = await dbContext.BodyweightRecords.Where(record => record.User == currentUser).OrderByDescending(record => record.Date).ToArrayAsync();

            return View(records);
        }

        [HttpPost]
        public async Task<IActionResult> EditRecords(DateTime[] Dates, float[] Weights)
        {
            if (Dates.Length != Weights.Length)
                return BadRequest();

            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            BodyweightRecord[] existingRecords = await dbContext.BodyweightRecords.Where(record => record.User == currentUser).ToArrayAsync();
            dbContext.BodyweightRecords.RemoveRange(existingRecords);


            for (int i = 0; i < Dates.Length; i++)
            {
                BodyweightRecord newRecord = new BodyweightRecord()
                {
                    User = currentUser,
                    Date = Dates[i],
                    Weight = Weights[i]
                };
                dbContext.BodyweightRecords.Add(newRecord);
            }

            await dbContext.SaveChangesAsync();
            return RedirectToAction("Summary");
        }

        [HttpPost]
        public async Task<IActionResult> AddTodayWeight(float Weight)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            BodyweightRecord newRecord = new BodyweightRecord()
            {
                User = currentUser,
                Date = DateTime.Today,
                Weight = Weight
            };

            dbContext.BodyweightRecords.Add(newRecord);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("Summary");
        }

        [HttpGet]
        public async Task<IActionResult> GetBodyweightData(int PreviousDays)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            var records = await dbContext.BodyweightRecords
                .Where(record => record.User == currentUser && record.Date >= DateTime.Today.AddDays(-PreviousDays))
                .OrderBy(record => record.Date)
                .ToArrayAsync();

            var result = records.Select(record => new { Date = record.Date.ToString("d"), Weight = record.Weight }).ToArray();

            return Json(result);
        }
    }
}
