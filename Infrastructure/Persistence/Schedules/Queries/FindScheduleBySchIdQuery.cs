using System;
using ApplicationCore.DTOs;
using MediatR;

namespace Infrastructure.Persistence.Schedules.Queries
{
	public class FindScheduleBySchIdQuery : IRequest<SingleScheduleResponse>
    {
		public int _schId { get; set; }

		public FindScheduleBySchIdQuery(int schId)
		{
			this._schId = schId;
		}
	}
}

