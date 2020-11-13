using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Controllers
{
    public class NutritionController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> AddFood()
        {
            return View();
        }
    }
}
