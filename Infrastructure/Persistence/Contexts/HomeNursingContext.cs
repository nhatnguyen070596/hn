using System;
using ApplicationCore.Aggregates;
using ApplicationCore.Entites;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Contexts
{
	public class HomeNursingContext : IdentityDbContext
    {
		public HomeNursingContext(DbContextOptions<HomeNursingContext> options)
			:base(options)
		{

		}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Additional configuration can be added here if necessary
        }

        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
    }
}

