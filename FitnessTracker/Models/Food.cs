using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Models
{
    public class Food
    {
        public long ID { get; set; }
        [Required]
        [ForeignKey("CreatedBy")]
        public string CreatedByID { get; set; }
        public FitnessUser CreatedBy { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int Calories { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Carbohydrates { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Protein { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int Fat { get; set; }
        [Required]
        [Range(0,int.MaxValue)]
        public int ServingSize { get; set; }
        [Required]
        public ServingUnit ServingUnit { get; set; }
    }
}
