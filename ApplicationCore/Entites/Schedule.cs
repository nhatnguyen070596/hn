using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Events.Schedules;

namespace ApplicationCore.Entites
{
    public class Schedule : EntityBase
    {
        [Required]
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int schId { get; set; }

        [Required]
        [Range(1, 4)]
        public required int SchType { get; set; }

        public required string StaffName { get; set; }

        [Required]
        public required int StaffId { get; set; }

        [ForeignKey("StaffId")]
        public Staff? Staff { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [MaxLength(30)]
        public string? Description { get; set; }

        public Schedule() { }
        public Schedule(int schType, string staffName,int staffId, bool isActive )
        {
            this.SchType = schType;
            this.StaffName = staffName;
            this.StaffId = staffId;
            this.IsActive = isActive;
            RaiseDomainEvent(new ScheduleCreatedDomainEvent(this.StaffId));
        }

        public void RaiseEventCreateSchedule()
        {
            RaiseDomainEvent(new ScheduleCreatedDomainEvent(this.StaffId));
        }
    }
}

