using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Models
{
    public class BodyweightTarget
    {
        public long ID { get; set; }
        public FitnessUser User { get; set; }
        public float TargetWeight { get; set; }
        public DateTime TargetDate { get; set; }
    }
}
