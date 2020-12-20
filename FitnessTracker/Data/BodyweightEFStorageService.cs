using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessTracker.Data
{
    public class BodyweightEFStorageService : IBodyweightStorageService
    {
        private ApplicationDbContext dbContext;

        public BodyweightEFStorageService(ApplicationDbContext DBContext)
        {
            this.dbContext = DBContext;
        }

        public async Task StoreBodyweightRecord(BodyweightRecord Record)
        {
            dbContext.BodyweightRecords.Add(Record);
            await dbContext.SaveChangesAsync();
        }

        public async Task StoreBodyweightRecords(IEnumerable<BodyweightRecord> Records)
        {
            dbContext.BodyweightRecords.AddRange(Records);
            await dbContext.SaveChangesAsync();
        }

        public async Task<BodyweightRecord[]> GetBodyweightRecords(FitnessUser User, bool AscendingOrder = false)
        {
            BodyweightRecord[] records = null;

            if (AscendingOrder == false)
            {
                records = await dbContext.BodyweightRecords
                    .Where(record => record.User == User)
                    .OrderByDescending(record => record.Date)
                    .ToArrayAsync();
            }
            else
            {
                records = await dbContext.BodyweightRecords
                    .Where(record => record.User == User)
                    .OrderBy(record => record.Date)
                    .ToArrayAsync();
            }

            return records;
        }

        public async Task<BodyweightTarget> GetBodyweightTarget(FitnessUser User)
        {
            BodyweightTarget result = await dbContext.BodyweightTargets.FirstOrDefaultAsync(target => target.User == User);
            return result;
        }

        public async Task DeleteExistingRecords(FitnessUser User)
        {
            BodyweightRecord[] existingRecords = await dbContext.BodyweightRecords.Where(record => record.User == User).ToArrayAsync();
            dbContext.BodyweightRecords.RemoveRange(existingRecords);
            await dbContext.SaveChangesAsync();
        }

        public async Task StoreBodyweightTarget(BodyweightTarget Target)
        {
            if (Target.ID == 0)
                dbContext.BodyweightTargets.Add(Target);
            else
                dbContext.BodyweightTargets.Update(Target);

            await dbContext.SaveChangesAsync();


        }
    }
}
