using FitnessTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Data
{
    public interface IGoalStorageService
    {
        public Task<Goal[]> GetAllGoals(FitnessUser User);
        public Task<Goal> GetGoalByID(FitnessUser User, long GoalID);
        public Task DeleteGoalByID(FitnessUser User, long GoalID);
        public Task StoreGoal(Goal Goal);
        public Task StoreGoalProgress(GoalProgress Progress);
        public Task<GoalProgress[]> GetGoalProgress(FitnessUser User, long GoalID, bool AscendingOrder = false);
    }
}
