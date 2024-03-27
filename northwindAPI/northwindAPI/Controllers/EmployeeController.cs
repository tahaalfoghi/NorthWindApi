using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using northwindAPI.Data.ModelDTO;
using northwindAPI.Models;
using northwindAPI.PatternService.UnitOfWork;
using northwindAPI.RepositoryService;

namespace northwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController:ControllerBase
    {
        private readonly IRepository<Employee> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public EmployeeController(IRepository<Employee> repo, IUnitOfWork uow, IMapper mapper)
        {
            _repo = repo;
            _uow = uow;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("getEmployees")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<EmployeeDTO>>> GetAllAsync()
        {
            var records = await _repo.GetAllAsync();
            if(records is null)
            throw new Exception($"Employees not found");

            var dto_records = _mapper.Map<List<EmployeeDTO>>(records);

            return dto_records;
        }
        [HttpGet]
        [Route("getEmployeeById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<EmployeeDTO>> GetById(int id)
        {
            var record = await _repo.GetById(id);
            if(record is null)
            throw new Exception($"the employee with id {id} not found");

            var dto_record = _mapper.Map<EmployeeDTO>(record);
            
            return dto_record;
        }
        [HttpPost]
        [Route("createEmployee")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateAsync(EmployeeDTO dto,CancellationToken token)
        {
            if(!ModelState.IsValid || dto is null)
            {
                throw new Exception($"invalid model");
            }
            var emp = _mapper.Map<Employee>(dto);
            await _repo.CreateAsync(emp);
            await _uow.SaveChangeAsync(token);
            return Ok();
        }
        [HttpPut]
        [Route("updateEmployee")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateAsync(int id, EmployeeDTO dto,CancellationToken token)
        {
            var record = await _repo.GetById(id);
            if(record is null || dto is null)
            {
                throw new Exception("record not found or the dto model is invalid");
            }
            var emp = _mapper.Map<Employee>(dto);
            await _repo.UpdateAsync(id,emp);
            await _uow.SaveChangeAsync(token);

            return Ok();
        }
        [HttpDelete]
        [Route("deleteEmployee")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken token)
        {
            var record = await _repo.GetById(id);
            if(record is null)
            throw new Exception($"employee with id {id} not found");

            await _repo.DeleteAsync(id);
            await _uow.SaveChangeAsync(token);

            return Ok();
        }
        [HttpPatch]
        [Route("updatePatch")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdatePatchAsync(int id, [FromBody] JsonPatchDocument<EmployeeDTO> patch,CancellationToken token)
        {
            var record  = await _repo.GetById(id);
            if(record is null || !ModelState.IsValid)
            throw new Exception($"record  with id:{id} not found or model is invalid");

            var dto_record = _mapper.Map<EmployeeDTO>(record);
            patch.ApplyTo(dto_record,ModelState);

            var employee = _mapper.Map<Employee>(dto_record);
            await _repo.UpdateAsync(id,employee);
            await _uow.SaveChangeAsync(token);
            
            return Ok(employee.EmployeeId);
        }
    }
}