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
    public class OrderController:ControllerBase
    {
        private readonly IRepository<Order> _repo;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public OrderController(IRepository<Order> repo, IMapper mapper, IUnitOfWork uow)
        {
            _repo = repo;
            _mapper = mapper;
            _uow = uow;
        }
        [HttpGet]
        [Route("getOrders")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAllAsync()
        {
            var records = await _repo.GetAllAsync();
            if(records is null)
            throw new Exception("orderDetail not found");

            var dto_records = _mapper.Map<List<OrderDTO>>(records);

            return dto_records;
        }
        [HttpGet]
        [Route("getOrderById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<OrderDTO>> GetById(int id)
        {
            var record = await _repo.GetById(id);
            if(record is null)
            throw new Exception($"order_detail {id} not found");

            var dto_record = _mapper.Map<OrderDTO>(record);

            return dto_record;
        }
        [HttpPost]
        [Route("addOrder")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateAsync(OrderDTO dto,CancellationToken token)
        {
            if(!ModelState.IsValid){
                throw new Exception("Invalid model");
            }
            var order = _mapper.Map<Order>(dto);

            await _repo.CreateAsync(order);
            await _uow.SaveChangeAsync(token);
            return Ok();
        }
        [HttpPut]
        [Route("updateOrder")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateAsync(int id, OrderDTO dto, CancellationToken token)
        {
            var record = await _repo.GetById(id);
            if(record is null || dto is null)
            {
                throw new Exception($"order with id {id} not found or the model invalid");
            }
            var order = _mapper.Map<Order>(dto);
            await _repo.UpdateAsync(id,order);
            await _uow.SaveChangeAsync(token);

            return Ok();
        }
        [HttpDelete]
        [Route("deleteOrder")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteAsync(int id,CancellationToken token)
        {
            var record = await _repo.GetById(id);
            if(record is null)
            {
                throw new Exception($"order {id} not found");
            }
            await _repo.DeleteAsync(id);
            await _uow.SaveChangeAsync(token);

            return Ok();
        }
        [HttpPatch]
        [Route("updatePatch {id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdatePatchAsync(int id, [FromBody] JsonPatchDocument<OrderDTO> patch,CancellationToken token)
        {
            var record  = await _repo.GetById(id);
            if(record is null || !ModelState.IsValid)
            throw new Exception($"record  with id:{id} not found or model is invalid");

            var dto_record = _mapper.Map<OrderDTO>(record);
            patch.ApplyTo(dto_record,ModelState);

            var order = _mapper.Map<Order>(dto_record);
            await _repo.UpdateAsync(id,order);
            await _uow.SaveChangeAsync(token);
            
            return Ok(order.OrderId);
        }
    }
}