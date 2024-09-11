using System;
using ApplicationCore.DTOs;
using ApplicationCore.Entites;
using ApplicationCore.Interfaces.DataAccess;
using ApplicationCore.Mappings;
using AutoMapper;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.DataAccess;
using Infrastructure.Persistence.Schedules.Command;
using Infrastructure.Persistence.Schedules.Handler;
using Infrastructure.Persistence.Schedules.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;

namespace IntegrationTests.Repositories
{
	public class ScheduleRepositoryTests : IClassFixture<SharedDatabaseFixture>
	{
        private readonly IMapper _mapper;
        private SharedDatabaseFixture Fixture { get; }

        public ScheduleRepositoryTests(SharedDatabaseFixture fixture)
        {
            Fixture = fixture;

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<GeneralProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public async Task GetAllScheduleHandler_ReturnsAllSchedule()
        {
            using (var context = Fixture.CreateContext())
            {
                var handler = new GetAllSchedulesHandler(context, _mapper);

                var schedule = await handler.Handle(new GetAllSchedulesQuery(), CancellationToken.None);

                Assert.Equal(10, schedule.Count);
            }
        }

        [Fact]
        public async Task SearchSchedule_ReturnsAllData()
        {
            using (var context = Fixture.CreateContext())
            {
                var handler = new SearchSchedulesByConditionHandler(context, _mapper);

                var schedule = await context.Schedules.ToListAsync();
                var staff = await context.Staffs.ToListAsync();
                var dataExpected = await context.Schedules.Include(o => o.Staff)
                    .Where(r => r.IsActive && r.SchType == 1 && r.Staff.StaffType == 1 && r.Staff.IsActive).ToListAsync();

                var data = await handler.Handle(new SearchSchedulesByConditionsQuery(new SearchSchedule
                {
                    SchType = 1,
                    StaffName = null,
                    StaffType = 1,
                    StaffId = 1,
                    IsActive = true
                }), CancellationToken.None);


                Assert.Equal(dataExpected.Count(), data.Count());
            }
        }

        [Fact]
        public async Task AddProductHandler_SavesCorrectData()
        {
            var schId = 0;

            var request = new CreateScheduleRequest
            {
                SchType = 4,
                StaffName = "string 11",
                Description = "Description 11",
                StaffId = 1,
                IsActive = true
            };

            using (var context = Fixture.CreateContext())
            {
                var mockUnitOfWork = new Mock<IUnitOfWork>();

                var mockPublisher = new Mock<IPublisher>();

                var unitOfWork = new UnitOfWork(mockPublisher.Object, context, _mapper);

                var handler = new AddScheduleCommandHandler(_mapper, unitOfWork);

                var schedule = await handler.Handle(new AddScheduleCommand(request), CancellationToken.None);

                schId = schedule.schId;
            }

            using (var context = Fixture.CreateContext())
            {
                var handler = new FindScheduleBySchIdHandler(context, _mapper);

                var schedule = await handler.Handle(new FindScheduleBySchIdQuery(schId), CancellationToken.None);

                Assert.NotNull(schedule);
                Assert.Equal(request.StaffId, schedule.StaffId);
                Assert.Equal(request.Description, schedule.Description);
                Assert.Equal(request.SchType, schedule.SchType);
                Assert.True(schedule.IsActive);
            }
        }
    }
}

