using System;
using ApplicationCore.DTOs;
using ApplicationCore.Entites;
using MediatR;

namespace Infrastructure.Persistence.Schedules.Queries
{
	public class GetAllSchedulesQuery : IRequest<List<SingleScheduleResponse>>
	{
	}
}

