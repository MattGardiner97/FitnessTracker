using System;
using System.Linq;
using System.Threading.Tasks;
using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Controllers
{
    public class SummaryModel
    {
        public BodyweightTarget Target { get; set; }
        public BodyweightRecord[] Records { get; set; }
    }

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

            BodyweightRecord[] records = new BodyweightRecord[300];
            float lastWeight = 90;
            Random r = new Random();
            for(int i = 299;i >= 0;i--)
            {
                float factor = r.Next(0, 4) == 3 ? 0.5f : -1f;
                float diff = (float)r.NextDouble() * factor;
                lastWeight += diff;
                records[i] = new BodyweightRecord()
                {
                    Date = DateTime.Today.AddDays(-i),
                    Weight = lastWeight
                };
            }

            SummaryModel resultModel = new SummaryModel()
            {
                Target = await dbContext.BodyweightTargets.Where(target => target.User == currentUser).FirstOrDefaultAsync(),
                //Records = await dbContext.BodyweightRecords.Where(record => record.User == currentUser).ToArrayAsync()
                Records = records
            };

            return View(resultModel);
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditRecords(DateTime[] Dates, float[] Weights)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            if (Dates.Length != Weights.Length || Dates.Length == 0 || Weights.Length == 0)
                return BadRequest();

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
    }
}
