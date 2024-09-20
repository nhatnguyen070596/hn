using ApplicationCore.DTOs;
using AutoMapper;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Schedules.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ApplicationCore.Extentions;
using ApplicationCore.Interfaces.DataAccess.Redis;
using Newtonsoft.Json;
using System.Linq;
using StackExchange.Redis;

namespace Infrastructure.Persistence.Schedules.Handler
{
    public class SearchSchedulesByConditionHandler : IRequestHandler<SearchSchedulesByConditionsQuery, List<SearchScheduleReponse>>
	{
        private readonly HomeNursingContext _homeNursingContext;
        private readonly IMapper _mapper;
        private readonly IRedisRepository _redis;
        private string _redisKey = string.Empty;
        public SearchSchedulesByConditionHandler(HomeNursingContext homeNursingContext, IMapper mapper
           , IRedisRepository redis)
		{
            _homeNursingContext = homeNursingContext;
            _mapper = mapper;
            _redis = redis;
            _redisKey = nameof(SearchSchedule);
        }

        public async Task<List<SearchScheduleReponse>> Handle(SearchSchedulesByConditionsQuery request, CancellationToken cancellationToken)
        {
            // If cached value is not null, deserialize it
            //var cachedData = _redis.GetValue(this._redisKey);
            //if (cachedData is not null)
            //{
            //    return JsonConvert.DeserializeObject<List<SearchScheduleReponse>>(cachedData) ?? new List<SearchScheduleReponse>();
            //}
            // If we have cached data, return it directly
            // Fetch data from the database since cache is empty
            var result = await _homeNursingContext.Schedules
                .Include(r => r.Staff)
                .Where(o => o.Staff.IsActive)
                .Select(s => new SearchScheduleReponse
                {
                    schId = s.schId,
                    SchType = s.SchType,
                    StaffName = s.StaffName,
                    StaffId = s.StaffId,
                    IsActive = s.IsActive,
                    StaffType = s.Staff.StaffType
                })
                .Where(request.searchData)
                .AsNoTracking()
                .ToListAsync();


            if (result.Count > 0)
            {
                _redis.SetValue(this._redisKey, JsonConvert.SerializeObject(result));
            }
            return result;

        }

    }
}

