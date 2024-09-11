using System;
using System.Net.Mime;
using ApplicationCore.DTOs;
using Infrastructure.Persistence.Schedules.Command;
using Infrastructure.Persistence.Schedules.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces("application/json")]
    //[Authorize]
    public class ScheduleController : ControllerBase
    {
        private readonly ILogger<ScheduleController> _logger;
        private readonly ISender mediator;
        public ScheduleController(ILogger<ScheduleController> logger, ISender mediator)
		{
            this.mediator = mediator;
            _logger = logger;
        }

        [HttpGet("GetAllSchedules")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<SingleScheduleResponse>))]
        public async Task<ActionResult> GetProductsAsync()
        {
            return Ok(await mediator.Send(new GetAllSchedulesQuery()));
        }

        [HttpPost("AddSchedule")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SingleScheduleResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> CreateAsync(CreateScheduleRequest request)
        {
            var schedule = await mediator.Send(new AddScheduleCommand(request));
            return StatusCode(201, schedule);
        }

        [HttpGet("GetScheduleById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SingleScheduleResponse))]
        public async Task<ActionResult> GetScheduleById(int schId)
        {
            return Ok(await mediator.Send(new FindScheduleBySchIdQuery(schId)));
        }

        [HttpPost("searchSchedulesByConditons")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchSchedule))]
        public async Task<ActionResult> searchSchedulesByConditons(SearchSchedule request)
        {
            return Ok(await mediator.Send(new SearchSchedulesByConditionsQuery(request)));
        }
    }
}

