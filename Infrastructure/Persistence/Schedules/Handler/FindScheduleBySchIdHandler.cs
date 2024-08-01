using System;
using ApplicationCore.DTOs;
using ApplicationCore.Entites;
using AutoMapper;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Schedules.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Schedules.Handler
{
	public class FindScheduleBySchIdHandler : IRequestHandler<FindScheduleBySchIdQuery, SingleScheduleResponse>
	{
        private readonly HomeNursingContext _homeNursingContext;
        private readonly IMapper _mapper;
        public FindScheduleBySchIdHandler(HomeNursingContext homeNursingContext, IMapper mapper)
        {
            _homeNursingContext = homeNursingContext;
            _mapper = mapper;
        }

        public async Task<SingleScheduleResponse> Handle(FindScheduleBySchIdQuery request, CancellationToken cancellationToken)
        {
            var schedule = await _homeNursingContext.Schedules
                .FindAsync(request._schId);
            return _mapper.Map<SingleScheduleResponse>(schedule);
        }
    }
}

