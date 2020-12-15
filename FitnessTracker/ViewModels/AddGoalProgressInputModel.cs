using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.ViewModels
{
    public class AddGoalProgressInputModel
    {
        [Required]
        [Range(0,long.MaxValue)]
        public long GoalID { get; set; }
        [Required]
        [MaxLength(20)]
        public string Type { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Range(0,500)]
        public float Weight { get; set; }
        [Range(0,100)]
        public int Reps { get; set; }
        [Range(0,int.MaxValue)]
        public int Quantity { get; set; }
        [Range(0, 24)]
        public int Hours { get; set; }
        [Range(0,60)]
        public int Minutes { get; set; }
        [Range(0,60)]
        public int Seconds { get; set; }
    }
}
