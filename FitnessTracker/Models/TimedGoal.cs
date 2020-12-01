using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Models
{
    public class TimedGoal : Goal
    {
        public TimeSpan Time { get; set; }
        public float Quantity { get; set; }
        public string QuantityUnit { get; set; }
    }
}
