using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Models
{
    public class BodyweightRecord
    {
        public int ID { get; set; }
        public FitnessUser User { get; set; }
        public DateTime Date { get; set; }
        public float Weight { get; set; }

    }
}
