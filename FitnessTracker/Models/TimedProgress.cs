using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Models
{
    public class TimedProgress : GoalProgress
    {
        public float Quantity { get; set; }
        [NotMapped]
        public string QuantityUnit { get { return ((TimedGoal)this.Goal).QuantityUnit; } }
        public TimeSpan Time { get; set; }
    }
}
