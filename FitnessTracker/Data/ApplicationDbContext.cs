
using System;
using System.Collections.Generic;
using System.Text;
using FitnessTracker.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Data
{
    public class ApplicationDbContext : IdentityDbContext<FitnessUser>
    {
        public DbSet<BodyweightRecord> BodyweightRecords { get; set; }
        public DbSet<BodyweightTarget> BodyweightTargets { get; set; }
        public DbSet<Food> UserFoods { get; set; }
        public DbSet<FoodRecord> FoodRecords { get; set; }
        public DbSet<NutritionTarget> NutritionTargets { get; set; }
        public DbSet<Goal> Goals { get; set; }
        public DbSet<WeightliftingGoal> WeightliftingGoals { get; set; }
        public DbSet<TimedGoal> TimedGoals { get; set; }
        public DbSet<GoalProgress> GoalProgressRecords { get; set; }
        public DbSet<WeightliftingProgress> WeightliftingProgressRecords { get; set; }
        public DbSet<TimedProgress> TimedProgressRecords { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FoodRecord>(entity =>
            {
                entity.HasOne(record => record.Food)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(record => record.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Food>(entity =>
            {
                entity.HasOne(food => food.CreatedBy)
                .WithMany()
                .OnDelete(DeleteBehavior.ClientCascade);
            });

            builder.Entity<NutritionTarget>(entity =>
            {
                entity.HasOne(record => record.User)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
