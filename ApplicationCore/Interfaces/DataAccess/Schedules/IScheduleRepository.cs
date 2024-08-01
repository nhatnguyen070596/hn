using System;
using ApplicationCore.DTOs;
using ApplicationCore.Entites;

namespace ApplicationCore.Interfaces.DataAccess.Schedules
{
	public interface IScheduleRepository : IRepository<Schedule>
	{
		Task<List<SingleScheduleResponse>> getSchedulesByStaffId(int staffId);

        Task<SingleScheduleResponse> FindScheduleBySchId(int schId);
    } 
}

