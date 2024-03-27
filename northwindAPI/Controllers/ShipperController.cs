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
    public class ShipperController:ControllerBase
    {
        private readonly IRepository<Shipper> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ShipperController(IRepository<Shipper> repo, IUnitOfWork uow, IMapper mapper)
        {
            _repo = repo;
            _uow = uow;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("getShippers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IQueryable<ShipperDTO>>> GetAllAsync()
        {
            var records = await _repo.GetAllAsync();
            if(records is null)
            throw new Exception("shippers not found");

            var dto_records = _mapper.Map<List<ShipperDTO>>(records);
            return Ok(dto_records);
        }
        [HttpGet]
        [Route("getShipperById {id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ShipperDTO>> GetByIdAsync(string id)
        {
            var record = await _uow.territoryRepository.GetByIdAsync(id);
            if(record is null)
            throw new Exception($"shipper with id:{id} not found");

            var dto_record = _mapper.Map<ShipperDTO>(record);
            return Ok(dto_record);
        }
        [HttpPost]
        [Route("addTerritory")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateAsync(ShipperDTO dto,CancellationToken token)
        {
            if(!ModelState.IsValid || dto is null)
            { 
                throw new Exception("invalid model");
            }
            var shipper = _mapper.Map<Shipper>(dto);
            await _repo.CreateAsync(shipper);
            await _uow.SaveChangeAsync(token);

            return Ok(shipper.ShipperId);
        }
        [HttpPut]
        [Route("updateShipper")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateAsync(int id, ShipperDTO dto, CancellationToken token)
        {
            var record = await _repo.GetById(id);
            if(record is null || id<=0 || !ModelState.IsValid || dto is null)
            throw new Exception($"shipper with id:{id} not found");

            var shipper = _mapper.Map<Shipper>(dto);
            await _repo.UpdateAsync(id,shipper);
            await _uow.SaveChangeAsync(token);
            return Ok(shipper.ShipperId);
        }
        [HttpDelete]
        [Route("deleteShipper")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken token)
        {
            var record = await _repo.GetById(id);
            if(record is null || id<=0)
            throw new Exception("record not found");

            await _repo.DeleteAsync(id);
            await _uow.SaveChangeAsync(token);
            return Ok(record.ShipperId);
        }
        [HttpPatch]
        [Route("updatePatch")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdatePatchAsync(int id, [FromBody] JsonPatchDocument<ShipperDTO> patch,CancellationToken token)
        {
            var record  = await _repo.GetById(id);
            if(record is null || !ModelState.IsValid)
            throw new Exception($"record  with id:{id} not found or model is invalid");

            var dto_record = _mapper.Map<ShipperDTO>(record);
            patch.ApplyTo(dto_record,ModelState);

            var shipper = _mapper.Map<Shipper>(dto_record);
            await _repo.UpdateAsync(id,shipper);
            await _uow.SaveChangeAsync(token);
            
            return Ok(shipper.ShipperId);
        }
    }
}