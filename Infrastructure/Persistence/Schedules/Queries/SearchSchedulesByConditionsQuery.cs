using System;
using ApplicationCore.DTOs;
using ApplicationCore.Interfaces.DataAccess.Filter;
using MediatR;

namespace Infrastructure.Persistence.Schedules.Queries
{
	public class SearchSchedulesByConditionsQuery : Filter,IRequest<List<SearchScheduleReponse>>
    {
        public SearchSchedule searchData = new SearchSchedule();
        public SearchSchedulesByConditionsQuery(SearchSchedule searchData)
        {
            this.searchData = searchData;
        }
    }
}

