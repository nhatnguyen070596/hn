using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.DTOs
{
    public class CreateStaffRequest
    {
        [StringLength(30, MinimumLength = 3)]
        public required string StaffName { get; set; }

        [Range(0, 3)]
        public required int StaffType { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }

    public class UpdateStaffRequest : CreateStaffRequest
    {
        [Required]
        public int StaffId { get; set; }
    }

    public class StaffResponse
    {
        public StaffResponse(int staffId, string staffName, int staffType, bool isActive)
        {
            StaffId = staffId;
            StaffName = staffName;
            StaffType = staffType;
            IsActive = isActive;

        }
        public int StaffId { get; set; }

        [StringLength(30, MinimumLength = 3)]
        public  string StaffName { get; set; }

        [Range(0, 3)]
        public  int StaffType { get; set; }

        [DefaultValue(true)]
        public bool IsActive { get; set; }
    }

    public class SingleStaffResponse : StaffResponse
    {
        public SingleStaffResponse(int staffId, string staffName, int staffType, bool isActive) : base(staffId, staffName, staffType, isActive)
        {
            StaffId = staffId;
            StaffName = staffName;
            StaffType = staffType;
            IsActive = isActive;
        }
    }
}

