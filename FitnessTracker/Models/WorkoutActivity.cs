using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Models
{
    public class WorkoutActivity
    {

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [MaxLength(30)]
        public string Quantity { get; set; }
        [Required]
        [Range(1,100)]
        public int Sets { get; set; } = 1;

        public int RestPeriodSeconds { get; set; } = 0;
    }
}
