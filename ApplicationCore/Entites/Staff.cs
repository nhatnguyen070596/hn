using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApplicationCore.Events.Schedules;

namespace ApplicationCore.Entites
{
    public class Staff : EntityBase
    {
        [Required]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StaffId { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public string StaffName { get; set; }

        [Range(0, 3)]
        public  int StaffType { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }

        public string? Description { get; set; }

        // Navigation property to represent the one-to-many relationship
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

        public Staff() { }

        public Staff(string staffName, int staffType, bool isActive, string description)
        {
            this.StaffName = staffName;
            this.StaffType = staffType;
            this.IsActive = isActive;
            this.Description = description;
        }
    }
}

