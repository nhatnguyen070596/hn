using System;
using ApplicationCore.Entites;
using ApplicationCore.Utils;
using Bogus;
using Infrastructure.Persistence.Contexts;

namespace ShareDataBase
{
    public static class ShareDatabaseMock
    {
        public static void SeedData(HomeNursingContext context)
        {
            try
            {
                context.Staffs.RemoveRange(context.Staffs);
                context.Schedules.RemoveRange(context.Schedules);

                var staffId = 1;
                var fakerStaff = new Faker<Staff>()
                    .RuleFor(o => o.StaffId, f => staffId++)
                    .RuleFor(o => o.StaffName, f => $"staff name {staffId}")
                    .RuleFor(o => o.Description, f => $"staff Description {staffId}")
                    .RuleFor(o => o.IsActive, f => true)
                    .RuleFor(o => o.StaffType, f => 1)
                    .RuleFor(o => o.CreatedAt, f => DateUtil.GetCurrentDate())
                    .RuleFor(o => o.UpdatedAt, f => DateUtil.GetCurrentDate());
                var staffs = fakerStaff.Generate(10);
                context.AddRange(staffs);
                context.SaveChanges();

                var schId = 1;
                var fakeSchedule = new Faker<Schedule>()
                    .RuleFor(o => o.SchType, f => f.Random.Number(1, 4))
                    .RuleFor(o => o.Description, f => $"Description {schId}")
                    .RuleFor(o => o.schId, f => schId++)
                    .RuleFor(o => o.IsActive, f => true)
                    .RuleFor(o => o.StaffId, f => context.Staffs.FirstOrDefault().StaffId)
                    .RuleFor(o => o.StaffName, f => context.Staffs.FirstOrDefault().StaffName)
                    .RuleFor(o => o.CreatedAt, f => DateUtil.GetCurrentDate())
                    .RuleFor(o => o.UpdatedAt, f => DateUtil.GetCurrentDate());
                var schedule = fakeSchedule.Generate(10);
                context.AddRange(schedule);
                context.SaveChanges();
            }
            catch (Exception ex )
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

