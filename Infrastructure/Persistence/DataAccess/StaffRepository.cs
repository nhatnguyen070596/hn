using System;
using ApplicationCore.DTOs;
using ApplicationCore.Entites;
using ApplicationCore.Interfaces.DataAccess.Staffs;
using AutoMapper;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DataAccess
{
	public class StaffRepository : Repository<Staff>, IStaffRepository
    {
        private readonly IMapper _mapper;
        public StaffRepository(HomeNursingContext dbContext, IMapper mapper) : base(dbContext)
        {
            _mapper = mapper;
        }

        public async Task<SingleStaffResponse> FindStaffById(int staffId)
        {
            var staff = await DbContext.Staffs.FindAsync(staffId);
            return _mapper.Map<SingleStaffResponse>(staff);
        }
    }
}

