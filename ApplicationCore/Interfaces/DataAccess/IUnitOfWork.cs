using System;
using ApplicationCore.Entites;
using ApplicationCore.Interfaces.DataAccess.Schedules;
using ApplicationCore.Interfaces.DataAccess.Staffs;

namespace ApplicationCore.Interfaces.DataAccess
{
    public interface IUnitOfWork : IDisposable
    {
        IScheduleRepository Schedules { get; }
        IStaffRepository Staffs { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken = default);

        Task ExecuteInTransactionAsync(Func<Task> action);
    }
}

