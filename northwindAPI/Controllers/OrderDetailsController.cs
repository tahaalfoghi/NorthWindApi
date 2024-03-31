using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using northwindAPI.Data.ModelDTO;
using northwindAPI.Models;
using northwindAPI.PatternService.UnitOfWork;
using northwindAPI.RepositoryService;

namespace northwindAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController:ControllerBase
    {
        private readonly IRepository<OrderDetail> _repo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public OrderDetailsController(IRepository<OrderDetail> repo, IMapper mapper, IUnitOfWork uow)
        {
            _repo = repo;
            _mapper = mapper;
            _uow = uow;
        }
        [HttpGet]
        [Route("getOrderDetail")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<OrderDetailDTO>>> GetAllAsync()
        {
            var records = await _repo.GetAllAsync();
            if(records is null)
            throw new Exception("orderDetail not found");

            var dto_records = _mapper.Map<List<OrderDetailDTO>>(records);

            return dto_records;
        }
        [HttpGet]
        [Route("getOrderDetailById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        
        public async Task<ActionResult<IEnumerable<OrderDetailDTO>>> GetById(int orderId)
        {
            var record = await _uow.orderDetailsRepository.GetByIdAsync(orderId);
            if(record is null)
            {
                throw new Exception($"record with order {orderId} not found");
            }
            var dto_record = _mapper.Map<List<OrderDetailDTO>>(record);
            return dto_record;
        }
        [HttpPatch]
        [Route("updatePatch {id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdatePatchAsync(int id, [FromBody] JsonPatchDocument<OrderDetailDTO> patch,CancellationToken token)
        {
            var record  = await _repo.GetById(id);
            if(record is null || !ModelState.IsValid)
            throw new Exception($"record  with id:{id} not found or model is invalid");

            var dto_record = _mapper.Map<OrderDetailDTO>(record);
            patch.ApplyTo(dto_record,ModelState);

            var orderDetail = _mapper.Map<OrderDetail>(dto_record);
            await _repo.UpdateAsync(id,orderDetail);
            await _uow.SaveChangeAsync(token);
            
            return Ok(orderDetail.OrderId);
        }
    }
}