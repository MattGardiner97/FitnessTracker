using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Models
{
    public class WorkoutSession
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [Range(1,28)]
        public int DayNumber { get; set; } = 1;

        public WorkoutActivity[] Activities { get; set; }
    }
}
