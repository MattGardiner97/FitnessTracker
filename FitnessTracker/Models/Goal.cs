using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Models
{
    public class Goal
    {
        public long ID { get; set; }
        public FitnessUser User { get; set; }
        public string Activity { get; set; }
    }
}
