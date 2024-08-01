using ApplicationCore.Interfaces.Events;

namespace ApplicationCore.Events.Schedules
{
    public record ScheduleCreatedDomainEvent(int staffId) : IDomainEvent;
}

