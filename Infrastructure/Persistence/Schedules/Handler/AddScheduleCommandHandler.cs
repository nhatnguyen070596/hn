using System;
using ApplicationCore.DTOs;
using ApplicationCore.Entites;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces.DataAccess;
using ApplicationCore.Utils;
using AutoMapper;
using Infrastructure.Persistence.Schedules.Command;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Schedules.Handler
{
    public class AddScheduleCommandHandler : IRequestHandler<AddScheduleCommand, SingleScheduleResponse>
	{
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public AddScheduleCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<SingleScheduleResponse> Handle(AddScheduleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var schedule = _mapper.Map<Schedule>(request._request);
                await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    schedule.IsActive = true;

                    schedule.CreatedAt = schedule.UpdatedAt = DateUtil.GetCurrentDate();

                    var staff = await _unitOfWork.Staffs.FindStaffById(schedule.StaffId);

                    if (staff is null)
                    {
                        throw new NotFoundException("Staff Id not Exist");
                    }
                    _unitOfWork.Schedules.Add(schedule);
                });

                return _mapper.Map<SingleScheduleResponse>(schedule);
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving the schedule. Please try again later.", ex);
            }
            catch (AutoMapperMappingException ex)
            {
                throw new AutoMapperException("Have something wrong with AutoMapper, please Check it", ex);
            } 
        }
    }
}

