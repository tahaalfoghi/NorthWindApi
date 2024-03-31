using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using northwindAPI.Data.ModelDTO;
using northwindAPI.Models;
using northwindAPI.PatternService.UnitOfWork;
using northwindAPI.RepositoryService;
using northwindAPI.PatternService.Repository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace northwindAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController:ControllerBase
    {
        private readonly IRepository<Customer> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerController> _logger;
        private readonly CustomerRepository _custRepo;
        public CustomerController(IRepository<Customer> repo, IUnitOfWork uow, IMapper mapper, ILogger<CustomerController> logger,CustomerRepository custRepo)
        {
            _repo = repo;
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _custRepo = custRepo;
        }
        [HttpGet]
        [Route("GetCustomers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAllAsync()
        {
            var records = await _repo.GetAllAsync();
            if(records is null)
            throw new Exception("records not found");

            var dto_records = _mapper.Map<List<CustomerDTO>>(records);

            return dto_records;
        }
        [HttpGet]
        [Route("GetCustomerById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<CustomerDTO>> GetById(string? id)
        {
            var record = await _repo.GetAsync(x=>x.CustomerId == id);
            if(record is null)
            throw new Exception($"Not found: id {id} id invalid id");

            var dto_record = _mapper.Map<CustomerDTO>(record);

            return dto_record;
        }
        [HttpPost]
        [Route("addCustomer")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateAsync(CustomerDTO dto,CancellationToken token)
        {
            if(!ModelState.IsValid)
            {
                throw new Exception("invalid model");
            }
            if(dto is null)
            {
                throw new Exception("invalid data for creating new customer");
            }

            var record = _mapper.Map<Customer>(dto);
            await _repo.CreateAsync(record);
            await _uow.SaveChangeAsync(token);
            return Ok();
        }
        [HttpPut]
        [Route("updateCustomer")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateAsync(string? id, CustomerDTO dto,CancellationToken token)
        {
            var record =  await _repo.GetAsync(x=>x.CustomerId == id);
            if(record is null)
            throw new Exception($"customer with id {id} not found");

            if(dto is null)
            throw new Exception("invalid data for updating");
            
            var cust = _mapper.Map<Customer>(dto);
            //await _custRepo.UpdateAsync(id,dto);
            await _uow.customerRepository.UpdateAsync(id,cust);
            await _uow.SaveChangeAsync(token);
            return Ok();
        }
        [HttpDelete]
        [Route("deleteCustomer")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteAsync(string? id , CancellationToken token)
        {
            var record = await _repo.GetAsync(x=>x.CustomerId == id);
            if(record is null)
            throw new Exception($"customer with id {id} not found");

            await _uow.customerRepository.DeleteAsync(id);
            return Ok();
        }
        [HttpPatch]
        [Route("updatePatch {id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdatePatchAsync(string id, [FromBody] JsonPatchDocument<CustomerDTO> patch,CancellationToken token)
        {
            var record  = await _repo.GetAsync(x=>x.CustomerId == id);
            if(record is null || !ModelState.IsValid)
            throw new Exception($"record  with id:{id} not found or model is invalid");

            var dto_record = _mapper.Map<CustomerDTO>(record);
            patch.ApplyTo(dto_record,ModelState);

            var customer = _mapper.Map<Customer>(dto_record);
            await _uow.customerRepository.UpdateAsync(id,customer);
            await _uow.SaveChangeAsync(token);
            
            return Ok(customer.CustomerId);
        }
    }
}