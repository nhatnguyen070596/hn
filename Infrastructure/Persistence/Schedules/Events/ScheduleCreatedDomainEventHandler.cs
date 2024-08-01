using System;
using ApplicationCore.DTOs;
using ApplicationCore.Entites;
using ApplicationCore.Events.Schedules;
using ApplicationCore.Exceptions;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.DataAccess;
using ApplicationCore.Utils;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Schedules.Event
{
	public sealed class ScheduleCreatedDomainEventHandler : INotificationHandler<ScheduleCreatedDomainEvent>
	{
        private readonly IUnitOfWork _unitOfWork;

        public ScheduleCreatedDomainEventHandler(IUnitOfWork unitOfWork)
		{
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ScheduleCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            try {
                await _unitOfWork.ExecuteInTransactionAsync(() => {
                    var staff = new Staff("Nhat", 1, true, "It's created by Nhat");
                    _unitOfWork.Staffs.Add(staff);
                    return Task.CompletedTask;
                });
            }
            catch (DbUpdateException ex)
            {
                throw new Exception("An error occurred while saving the new Staff. Please try again later.", ex);
            }
       
        }
    }
}

