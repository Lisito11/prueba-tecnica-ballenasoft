using System;
using api.DTOs;
using api.Helpers;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class InsuranceController : ControllerBase
    {
        private readonly IInsuranceService _insuranceService;

        public InsuranceController(IInsuranceService insuranceService)
        {
            _insuranceService = insuranceService;
        }


        [HttpGet("{id}", Name = "UserById")]
        public async Task<IActionResult> GetById(int id)
        {

            var response = await _insuranceService.GetById(id)!;
            return response.Succeeded is false ? NotFound(response) : Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PaginationFilter filter)
        {

            var response = await _insuranceService.GetAll(filter);
            return response.Succeeded is false ? StatusCode(500, response) : Ok(response);
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] InsuranceCreationDTO insurance)
        {
            ResponseBase<Insurance> response = new ResponseBase<Insurance>();

            if (insurance is null)
            {
                response.Succeeded = false;
                response.Error = "Debe de enviar la aseguradora";
                return BadRequest(response);
            }
            if (!ModelState.IsValid)
            {
                response.Succeeded = false;
                response.Error = "Debe de enviar correctamente la aseguradora";
                return BadRequest(response);
            }

            response = await _insuranceService.Create(insurance);

            if (response.StatusCode == 400)
            {
                return BadRequest(response);
            }

            return CreatedAtRoute("UserById", new { id = response.Data!.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] InsuranceCreationDTO insurance)
        {
            ResponseBase<bool> response = new ResponseBase<bool>();

            if (insurance is null)
            {
                response.Succeeded = false;
                response.Error = "Debe de enviar la aseguradora";
                return BadRequest(response);
            }
            if (!ModelState.IsValid)
            {
                response.Succeeded = false;
                response.Error = "Debe de enviar correctamente la aseguradora";
                return BadRequest(response);
            }

            response = await _insuranceService.Update(id, insurance);

            if (response.StatusCode == 400) {
                return BadRequest(response);
            } 

            return response.Succeeded == false ? NotFound(response) : NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            ResponseBase<bool> response = await _insuranceService.Delete(id);

            return response.Succeeded == false ? NotFound(response) : NoContent();

        }

    }
}

