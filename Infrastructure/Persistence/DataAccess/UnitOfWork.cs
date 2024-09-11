using ApplicationCore.Aggregates;
using ApplicationCore.Interfaces.DataAccess;
using ApplicationCore.Interfaces.DataAccess.Schedules;
using ApplicationCore.Interfaces.DataAccess.Staffs;
using ApplicationCore.Interfaces.Events;
using AutoMapper;
using Infrastructure.Persistence.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Persistence.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IPublisher _publisher;

        private readonly HomeNursingContext _context;

        private readonly IMapper _mapper;

        private IDbContextTransaction? _currentTransaction;

        private IScheduleRepository? _scheduleRepository;
        private IStaffRepository? _staffRepository;

        public IScheduleRepository Schedules => _scheduleRepository ??= new ScheduleRepository(_context, _mapper);
        public IStaffRepository Staffs => _staffRepository ??= new StaffRepository(_context, _mapper);

        public UnitOfWork( IPublisher publisher,HomeNursingContext context,IMapper mapper)
        {
                _publisher = publisher;
                _context = context;
                _mapper = mapper;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var domainEntities = GetDomainEntities(_context);

            var domainEvents = GetDomainEvents(domainEntities);

            domainEntities.ForEach(entity => entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                 _publisher.Publish(domainEvent, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
           
        }

        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            if (_currentTransaction != null)
            {
                // If a transaction is already in progress, just execute the action
                await action();
                return;
            }

            await using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                _currentTransaction = transaction;
                try
                {
                    await action();
                }
                catch
                {
                    await _currentTransaction.RollbackAsync();
                    throw;
                }
                finally
                {
                    await SaveChangesAsync(); // Ensure all changes are saved before committing
                    await _currentTransaction.CommitAsync();
                    await DisposeTransactionAsync();
                }
            }
        }

        private async Task DisposeTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        private List<AggregateRoot> GetDomainEntities(HomeNursingContext context)
        {
            return context.ChangeTracker
                          .Entries<AggregateRoot>()
                          .Select(e => e.Entity)
                          .ToList();
        }

        private List<IDomainEvent> GetDomainEvents(List<AggregateRoot> entities)
        {
            return entities
                       .Where(e => e.DomainEvents.Any())
                       .SelectMany(e => e.DomainEvents)
                       .ToList();
        }
    }
}

