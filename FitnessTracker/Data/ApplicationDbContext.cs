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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
