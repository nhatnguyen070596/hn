using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Entites;

namespace ApplicationCore.DTOs
{
    public class CreateScheduleRequest
    {
        [Required]
        [Range(1, 4)]
        public required int SchType { get; set; }

        public required string StaffName { get; set; }

        [Required]
        [DefaultValue(1)]
        public required int StaffId { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        [MaxLength(30)]
        public string? Description { get; set; }
    }

    public class UpdateScheduleRequest : CreateScheduleRequest
    {
        [Required]
        public int schId { get; set; }
    }

    public class ScheduleResponse 
    {
        public int schId { get; set; }

        public required int SchType { get; set; }

        public required string StaffName { get; set; }

        public required int StaffId { get; set; }

        public bool IsActive { get; set; }

        public string? Description { get; set; }
    }

    public class SingleScheduleResponse : ScheduleResponse
    {

    }
}

