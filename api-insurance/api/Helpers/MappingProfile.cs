using System;
using api.DTOs;
using api.Models;
using AutoMapper;

namespace api.Helpers
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
            CreateMap<InsuranceCreationDTO, Insurance>();
        }
	}
}

