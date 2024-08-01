
using ApplicationCore.DTOs;
using AutoMapper;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Schedules.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Schedules.Handler
{
    public class GetAllSchedulesHandler : IRequestHandler<GetAllSchedulesQuery, List<SingleScheduleResponse>>
	{
        private readonly HomeNursingContext _homeNursingContext;
        private readonly IMapper _mapper;
        public GetAllSchedulesHandler(HomeNursingContext homeNursingContext, IMapper mapper)
		{
            _homeNursingContext = homeNursingContext;
            _mapper = mapper;
        }

        public async Task<List<SingleScheduleResponse>> Handle(GetAllSchedulesQuery request, CancellationToken cancellationToken)
        {
            return await _homeNursingContext.Schedules
                            .Where(s => s.IsActive)
                            .Select(s => _mapper.Map<SingleScheduleResponse>(s)).AsNoTracking()
                            .ToListAsync();
        }
    }
}

