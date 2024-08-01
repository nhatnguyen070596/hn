using System;
using ApplicationCore.DTOs;
using ApplicationCore.Entites;
using MediatR;

namespace Infrastructure.Persistence.Staffs.Queries
{
    public class FindStaffById : IRequest<Staff>
    {
    }
}

