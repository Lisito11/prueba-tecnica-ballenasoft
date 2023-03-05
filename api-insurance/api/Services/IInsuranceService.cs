using System;
using api.DTOs;
using api.Helpers;
using api.Models;

namespace api.Services
{
	public interface IInsuranceService
	{
        Task<ResponseBase<List<Insurance>>> GetAll(PaginationFilter filter);
        Task<ResponseBase<Insurance>> GetById(int id);
        Task<ResponseBase<Insurance>> Create(InsuranceCreationDTO insurance);
        Task<ResponseBase<bool>> Update(int id, InsuranceCreationDTO insurance);
        Task<ResponseBase<bool>> Delete(int id);
    }
}

