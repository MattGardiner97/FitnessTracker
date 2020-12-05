using FitnessTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.ViewModels
{
    public class GoalViewModel
    {
        public Goal Goal{get;set;}
        public GoalProgress[] Progress { get; set; }


    }
}
