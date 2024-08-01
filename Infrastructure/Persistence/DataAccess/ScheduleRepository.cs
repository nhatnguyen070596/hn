using System;
using ApplicationCore.DTOs;
using ApplicationCore.Entites;
using ApplicationCore.Interfaces.DataAccess.Schedules;
using AutoMapper;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DataAccess
{
    public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
    {
        private readonly IMapper _mapper;
        public ScheduleRepository(HomeNursingContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        public async Task<SingleScheduleResponse> FindScheduleBySchId(int schId)
        {
            var schedule = await DbContext.Schedules.FindAsync(schId);
            return _mapper.Map<SingleScheduleResponse>(schedule);
        }

        async Task<List<SingleScheduleResponse>> IScheduleRepository.getSchedulesByStaffId(int staffId)
        {
            return await DbContext.Schedules
                 .Where(s => s.StaffId == staffId && s.IsActive)
                 .Select(s => _mapper.Map<SingleScheduleResponse>(s))
                 .ToListAsync();
        }
    }
}

