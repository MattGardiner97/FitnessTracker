using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Models
{
    public class Food
    {
        public long ID { get; set; }
        public FitnessUser CreatedBy { get; set; }
        public string Name { get; set; }
        public int Calories { get; set; }
        public int Carbohydrates{ get; set; }
        public int Protein { get; set; }
        public int Fat { get; set; }
        public int ServingSize { get; set; }
        public ServingUnit ServingUnit { get; set; }
    }
}
