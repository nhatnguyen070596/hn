using System;
using System.Linq;
using ApplicationCore.DTOs;
using AutoMapper;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Schedules.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Extentions;

namespace Infrastructure.Persistence.Schedules.Handler
{
	public class SearchSchedulesByConditionHandler : IRequestHandler<SearchSchedulesByConditionsQuery, List<SearchScheduleReponse>>
	{
        private readonly HomeNursingContext _homeNursingContext;
        private readonly IMapper _mapper;
        public SearchSchedulesByConditionHandler(HomeNursingContext homeNursingContext, IMapper mapper)
		{
            _homeNursingContext = homeNursingContext;
            _mapper = mapper;
        }

        public async Task<List<SearchScheduleReponse>> Handle(SearchSchedulesByConditionsQuery request, CancellationToken cancellationToken)
        {
            var result = await _homeNursingContext.Schedules
                      .Include(r => r.Staff).Where(o => o.Staff.IsActive)
                      .Select(s => new SearchScheduleReponse {
                          schId = s.schId ,
                          SchType = s.SchType,
                          StaffName = s.StaffName,
                          StaffId = s.StaffId,
                          IsActive = s.IsActive,
                          StaffType = s.Staff.StaffType 
                      }).Where(request.searchData).AsNoTracking().ToListAsync();
            return result;

          //  var result = await _homeNursingContext.Schedules
          //.Include(r => r.Staff).Where(o => o.Staff.IsActive)
          //.Select(s => _mapper.Map<SearchScheduleReponse>(s)).Where(request.searchData).AsNoTracking().ToListAsync();
          //  return result;
        }

    }
}

