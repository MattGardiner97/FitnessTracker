using FitnessTracker.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Models
{
    public class NutritionTarget
    {
        public long ID { get; set; }
        [Required]
        public FitnessUser User { get; set; }
        [Required]
        public int DailyCalories { get; set; }
        [Required]
        public int DailyCarbohydrates { get; set; }
        [Required]
        public int DailyProtein { get; set; }
        [Required]
        public int DailyFat { get; set; }

    }
}
