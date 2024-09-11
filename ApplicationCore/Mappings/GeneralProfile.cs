using System;
using ApplicationCore.DTOs;
using ApplicationCore.Entites;
using AutoMapper;

namespace ApplicationCore.Mappings
{
	public class GeneralProfile : Profile
	{
		public GeneralProfile()
		{
            //Schedules mapper

            CreateMap<CreateScheduleRequest, Schedule>()
              .ForMember(dest =>
                  dest.schId,
                  opt => opt.Ignore()
              )
              .ForMember(dest =>
                  dest.CreatedAt,
                  opt => opt.Ignore()
              )
              .ForMember(dest =>
                  dest.UpdatedAt,
                  opt => opt.Ignore()
              );

            CreateMap<Schedule, SingleScheduleResponse>();


            CreateMap<Schedule, SingleScheduleResponse>();

            CreateMap<Schedule, SearchSchedule>();


            CreateMap<Schedule, SearchScheduleReponse>()
                .ForMember(dest => dest.StaffType, opt => opt.MapFrom(src => src.Staff.StaffType));


            ///staff mappper
            ///

            CreateMap<CreateStaffRequest, Staff>()
              .ForMember(dest =>
                  dest.StaffId,
                  opt => opt.Ignore()
              );

            CreateMap<Staff, StaffResponse>();
            CreateMap<Staff, SingleStaffResponse>();
        }
	}
}

