using System;
using System.Threading.Tasks;
using FitnessTracker.Data;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FitnessTracker.Controllers
{
    public class BodyweightController : Controller
    {
        private ApplicationDbContext dbContext;
        private UserManager<FitnessUser> userManager;

        public BodyweightController(ApplicationDbContext DBContext,UserManager<FitnessUser> UserManager)
        {
            dbContext = DBContext;
            userManager = UserManager;
        }

        public IActionResult Summary()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBodyweights(DateTime[] Dates, float[] Weights)
        {
            FitnessUser currentUser = await userManager.GetUserAsync(HttpContext.User);

            if (Dates.Length != Weights.Length || Dates.Length == 0 || Weights.Length == 0)
                return BadRequest();

            for(int i = 0; i  <Dates.Length;i++)
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
