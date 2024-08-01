using System;
using ApplicationCore.DTOs;
using MediatR;

namespace Infrastructure.Persistence.Schedules.Command
{
	public class AddScheduleCommand : IRequest<SingleScheduleResponse>
    {
        public CreateScheduleRequest _request { get; set; }

        public AddScheduleCommand(CreateScheduleRequest request)
        {
            _request = request;
        }
    }
}

