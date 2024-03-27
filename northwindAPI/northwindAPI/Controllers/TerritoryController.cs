using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using northwindAPI.Data.ModelDTO;
using northwindAPI.Models;
using northwindAPI.PatternService.UnitOfWork;
using northwindAPI.RepositoryService;

namespace northwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TerritoryController:ControllerBase
    {
        private readonly IRepository<Territory> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public TerritoryController(IRepository<Territory> repo, IUnitOfWork uow, IMapper mapper)
        {
            _repo = repo;
            _uow = uow;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("getTerritories")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IQueryable<TerritoryDTO>>> GetAllAsync()
        {
            var records = await _repo.GetAllAsync();
            if(records is null)
            throw new Exception("regions not found");

            var dto_records = _mapper.Map<List<TerritoryDTO>>(records);
            return Ok(dto_records);
        }
        [HttpGet]
        [Route("getTerritoryById {id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<TerritoryDTO>> GetByIdAsync(string id)
        {
            var record = await _uow.territoryRepository.GetByIdAsync(id);
            if(record is null)
            throw new Exception($"territory with id:{id} not found");

            var dto_record = _mapper.Map<TerritoryDTO>(record);
            return Ok(dto_record);
        }
        [HttpPost]
        [Route("addTerritory")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateAsync(TerritoryDTO dto,CancellationToken token)
        {
            if(!ModelState.IsValid || dto is null)
            { 
                throw new Exception("invalid model");
            }
            var territory = _mapper.Map<Territory>(dto);
            await _repo.CreateAsync(territory);
            await _uow.SaveChangeAsync(token);

            return Ok(territory.TerritoryId);
        }
        [HttpPut]
        [Route("updateTerritory")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateAsync(string id, TerritoryDTO dto, CancellationToken token)
        {
            var record = await _uow.territoryRepository.GetByIdAsync(id);
            if(record is null || id.IsNullOrEmpty() || !ModelState.IsValid || dto is null)
            throw new Exception($"region with id:{id} not found");

            var territory = _mapper.Map<Territory>(dto);
            await _uow.territoryRepository.UpdateAsync(id,territory);
            await _uow.SaveChangeAsync(token);
            return Ok(territory.TerritoryId);
        }
        [HttpDelete]
        [Route("deleteTerritory")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteAsync(string id, CancellationToken token)
        {
            var record = await _uow.territoryRepository.GetByIdAsync(id);
            if(record is null || id.IsNullOrEmpty())
            throw new Exception("record not found");

            await _uow.territoryRepository.DeleteAsync(id);
            await _uow.SaveChangeAsync(token);
            return Ok(record.TerritoryId);
        }
        [HttpPatch]
        [Route("updatePatch {id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdatePatchAsync(string id, [FromBody] JsonPatchDocument<TerritoryDTO> patch,CancellationToken token)
        {
            var record  = await _uow.territoryRepository.GetByIdAsync(id);
            if(record is null || !ModelState.IsValid)
            throw new Exception($"record  with id:{id} not found or model is invalid");

            var dto_record = _mapper.Map<TerritoryDTO>(record);
            patch.ApplyTo(dto_record,ModelState);

            var territory = _mapper.Map<Territory>(dto_record);
            await _uow.territoryRepository.UpdateAsync(id,territory);
            await _uow.SaveChangeAsync(token);
            
            return Ok(territory.TerritoryId);
        }
    }
}