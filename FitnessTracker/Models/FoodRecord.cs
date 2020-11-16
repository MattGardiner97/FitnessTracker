using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Models
{
    public class FoodRecord
    {
        public long ID { get; set; }

        [Required]
        public FitnessUser User { get; set; }
        [Required]
        [ForeignKey("Food")]
        public long FoodID { get; set; }
        public Food Food { get; set; }
        [Required]
        public DateTime ConsumptionDate { get; set; }
        [Required]
        public float Quantity { get; set; }
    }
}
