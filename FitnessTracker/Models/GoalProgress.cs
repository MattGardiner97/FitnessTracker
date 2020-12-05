using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Models
{
    public class GoalProgress
    {
        public long ID { get; set; }
        public FitnessUser User { get; set; }
        public Goal Goal { get; set; }
        public DateTime Date { get; set; }

    }
}
