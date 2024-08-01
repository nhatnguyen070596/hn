using System;
using ApplicationCore.DTOs;
using ApplicationCore.Entites;

namespace ApplicationCore.Interfaces.DataAccess.Staffs
{
    public interface IStaffRepository : IRepository<Staff>
    {
        Task<SingleStaffResponse> FindStaffById(int staffId);
    }
}

