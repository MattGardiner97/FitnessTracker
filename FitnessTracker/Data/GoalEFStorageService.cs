using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Data
{
    public class GoalEFStorageService : IGoalStorageService
    {
        private ApplicationDbContext dbContext;

        public GoalEFStorageService(ApplicationDbContext DBContext)
        {
            this.dbContext = DBContext;
        }

        public async Task DeleteGoalByID(FitnessUser User, long GoalID)
        {
            Goal existingGoal = await dbContext.Goals.FirstOrDefaultAsync(goal => goal.ID == GoalID && goal.User == User);
            if (existingGoal == null)
                return;

            dbContext.Goals.Remove(existingGoal);

            await dbContext.SaveChangesAsync();
        }

        public async Task<Goal[]> GetAllGoals(FitnessUser User)
        {
            Goal[] result = await dbContext.Goals.Where(goal => goal.User == User).ToArrayAsync();
            return result;
        }

        public async Task<Goal> GetGoalByID(FitnessUser User, long GoalID)
        {
            Goal result = await dbContext.Goals.FirstOrDefaultAsync(goal => goal.ID == GoalID && goal.User == User);
            return result;
        }

        public async Task<GoalProgress[]> GetGoalProgress(FitnessUser User, long GoalID, bool AscendingOrder = false)
        {
            var query= dbContext.GoalProgressRecords
                .Where(record => record.Goal.ID == GoalID && record.User == User);
            if (AscendingOrder == true)
                query = query.OrderBy(record => record.Date);
            else
                query = query.OrderByDescending(record => record.Date);

            GoalProgress[] result = await query.ToArrayAsync();

            return result;
        }

        public async Task StoreGoal(Goal Goal)
        {
            if (Goal.ID == 0)
                dbContext.Goals.Add(Goal);
            else
                dbContext.Goals.Update(Goal);

            await dbContext.SaveChangesAsync();
        }

        public async Task StoreGoalProgress(GoalProgress Progress)
        {
            dbContext.GoalProgressRecords.Add(Progress);
            await dbContext.SaveChangesAsync();
        }
    }
}
