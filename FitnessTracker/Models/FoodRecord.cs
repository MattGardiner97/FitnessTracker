using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Models
{
    public class FoodRecord
    {
        public long ID { get; set; }
        public Food Food { get; set; }
        public DateTime ConsumptionDate { get; set; }
        public float Quantity { get; set; }
    }
}
