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
        private IBodyweightStorageService storageService;
        private UserManager<FitnessUser> userManager;

        public BodyweightController(IBodyweightStorageService StorageService, UserManager<FitnessUser> UserManager)
        {
            this.storageService = StorageService;
            this.userManager = UserManager;
        }

        private async Task<FitnessUser> GetUser()
        {
            return await userManager.GetUserAsync(HttpContext.User);
        }

        public async Task<IActionResult> Summary()
        {
            FitnessUser currentUser = await GetUser();

            IEnumerable<BodyweightRecord> records = await storageService.GetBodyweightRecords(currentUser);
            BodyweightTarget target = await storageService.GetBodyweightTarget(currentUser);

            BodyweightSummaryViewModel viewModel = new BodyweightSummaryViewModel(records, target);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EditTarget()
        {
            FitnessUser currentUser = await GetUser();

            BodyweightTarget target = await storageService.GetBodyweightTarget(currentUser);

            return View(target);
        }

        [HttpPost]
        public async Task<IActionResult> EditTarget(float TargetWeight, DateTime TargetDate)
        {
            if (TargetWeight <= 0 || TargetWeight >= 200 || TargetDate <= DateTime.Today)
                return BadRequest();

            FitnessUser currentUser = await GetUser();


            BodyweightTarget newTarget = await storageService.GetBodyweightTarget(currentUser);
            if (newTarget == null)
            {
                newTarget = new BodyweightTarget()
                {
                    User = currentUser
                };
            }
            newTarget.TargetWeight = TargetWeight;
            newTarget.TargetDate = TargetDate;
            await storageService.StoreBodyweightTarget(newTarget);

            return RedirectToAction("Summary");
        }

        [HttpGet]
        public async Task<IActionResult> EditRecords()
        {
            FitnessUser currentUser = await GetUser();

            BodyweightRecord[] records = await storageService.GetBodyweightRecords(currentUser);

            return View(records);
        }

        [HttpPost]
        public async Task<IActionResult> EditRecords(DateTime[] Dates, float[] Weights)
        {
            if (Dates == null || Weights == null)
                return BadRequest();
            if (Dates.Length != Weights.Length)
                return BadRequest();

            for (int i = 0; i < Dates.Length; i++)
            {
                if (Weights[i] <= 0 || Weights[i] >= 200)
                    return BadRequest();
            }

            FitnessUser currentUser = await GetUser();

            await storageService.DeleteExistingRecords(currentUser);

            BodyweightRecord[] records = new BodyweightRecord[Dates.Length];
            for (int i = 0; i < Dates.Length; i++)
            {
                BodyweightRecord newRecord = new BodyweightRecord()
                {
                    User = currentUser,
                    Date = Dates[i],
                    Weight = Weights[i]
                };
                records[i] = newRecord;
            }

            await storageService.StoreBodyweightRecords(records);
            return RedirectToAction("Summary");
        }

        [HttpPost]
        public async Task<IActionResult> AddTodayWeight(float Weight)
        {
            if (Weight <= 0 || Weight >= 200)
                return BadRequest();

            FitnessUser currentUser = await GetUser();

            BodyweightRecord newRecord = new BodyweightRecord()
            {
                User = currentUser,
                Date = DateTime.Today,
                Weight = Weight
            };

            await storageService.StoreBodyweightRecord(newRecord);
            return RedirectToAction("Summary");
        }

        [HttpGet]
        public async Task<IActionResult> GetBodyweightData(int PreviousDays)
        {
            FitnessUser currentUser = await GetUser();

            BodyweightRecord[] records = await storageService.GetBodyweightRecords(currentUser, true);

            var result = records.Select(record => new { Date = record.Date.ToString("d"), Weight = record.Weight }).ToArray();

            return Json(result);
        }
    }
}
