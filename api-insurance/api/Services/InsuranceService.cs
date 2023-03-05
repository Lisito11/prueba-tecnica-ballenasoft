using System;
using api.DTOs;
using api.Helpers;
using api.Models;
using api.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace api.Services
{
	public class InsuranceService : IInsuranceService
	{
        private readonly IInsuranceRepository _repository;
        private readonly IMapper _mapper;

        public InsuranceService(IInsuranceRepository repository, IMapper mapper)
		{
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ResponseBase<Insurance>> Create(InsuranceCreationDTO insurance)
        {
            ResponseBase<Insurance> response = new ResponseBase<Insurance>();

            try
            {
                var insuranceMapped = _mapper.Map<Insurance>(insurance);

                if (insuranceMapped.Name!.Length > 45)
                {
                    response.Succeeded = false;
                    response.StatusCode = 400;
                    response.Error = "El nombre no puede ser mayor de 45 caracteres.";
                    return response;
                }


                if (insuranceMapped.Fee <= 0 || insuranceMapped.Fee > 0.25) {
                    response.Succeeded = false;
                    response.StatusCode = 400;
                    response.Error = "La aseguradora no puede cobrar menos del 0% ni mas del 25% de comisión";
                    return response;
                }

                await _repository.Create(insuranceMapped);
                await _repository.Save();
                response = new ResponseBase<Insurance>(insuranceMapped);
                return response;
            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Error = ex.Message;
                return response;
            }
        }

        public async Task<ResponseBase<bool>> Delete(int id)
        {
            ResponseBase<bool> response = new ResponseBase<bool>();

            try
            {
                var insuranceDB = await _repository.getById(id)!;

                if (insuranceDB is null)
                {
                    response.Succeeded = false;
                    response.StatusCode = 404;
                    response.Error = "No se encuentra la aseguradora";
                    return response;
                }
                _repository.Delete(insuranceDB);
                await _repository.Save();
                return response;
            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Error = ex.Message;
                return response;
            }
        }

        public async Task<ResponseBase<List<Insurance>>> GetAll(PaginationFilter filter)
        {
            try
            {

                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

                var pagedData = await _repository.getAll()
                    .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                    .Take(validFilter.PageSize)
                    .ToListAsync();

                var totalRecords = _repository.getAll().Count();


                return PaginationResponse<Insurance>.CreatePaginationReponse(pagedData, validFilter, totalRecords);

            }
            catch (Exception ex)
            {
                return new PaginationResponse<List<Insurance>>() { Succeeded = false, Error = ex.Message };
            }

        }

        public async Task<ResponseBase<Insurance>> GetById(int id)
        {

            ResponseBase<Insurance> response = new ResponseBase<Insurance>();

            try
            {

                var insuranceDB = await _repository.getById(id)!;

                if (insuranceDB is null)
                {
                    response.Succeeded = false;
                    response.Error = "No hay aseguradora con ese nombre";
                    response.StatusCode = 404;
                    return response;
                }

                response.Data = insuranceDB;

                return response;
            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Error = ex.Message;
                return response;
            }
        }

        public async Task<ResponseBase<bool>> Update(int id, InsuranceCreationDTO insurance)
        {
            ResponseBase<bool> response = new ResponseBase<bool>();

            try
            {
                var insuranceDB = await _repository.getById(id)!;

                if (insuranceDB is null)
                {
                    response.Succeeded = false;
                    response.StatusCode = 404;
                    response.Error = "No se encuentra la aseguradora";
                    return response;
                }
                var insuranceUpdated = _mapper.Map(insurance, insuranceDB!);


                if (insuranceUpdated.Name!.Length > 45)
                {
                    response.Succeeded = false;
                    response.StatusCode = 400;
                    response.Error = "El nombre no puede ser mayor de 45 caracteres.";
                    return response;
                }

                if (insuranceUpdated.Fee <= 0 || insuranceUpdated.Fee > 0.25)
                {
                    response.Succeeded = false;
                    response.StatusCode = 400;
                    response.Error = "La aseguradora no puede cobrar menos del 0% ni mas del 25% de comisión";
                    return response;
                }
                _repository.Update(insuranceUpdated);
                await _repository.Save();
                return response;
            }
            catch (Exception ex)
            {
                response.Succeeded = false;
                response.Error = ex.Message;
                return response;
            }
        }
    }
}

