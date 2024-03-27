using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using northwindAPI.Data.ModelDTO;
using northwindAPI.Models;
using northwindAPI.PatternService.UnitOfWork;
using northwindAPI.RepositoryService;

namespace northwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController:ControllerBase
    {
        private readonly IRepository<Region> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public RegionController(IRepository<Region> repo, IUnitOfWork uow, IMapper mapper)
        {
            _repo = repo;
            _uow = uow;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("getRegions")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IQueryable<RegionDTO>>> GetAllAsync()
        {
            var records = await _repo.GetAllAsync();
            if(records is null)
            throw new Exception("regions not found");

            var dto_records = _mapper.Map<List<RegionDTO>>(records);
            return Ok(dto_records);
        }
        [HttpGet]
        [Route("getRegionById {id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<RegionDTO>> GetByIdAsync(int id)
        {
            var record = await _repo.GetById(id);
            if(record is null)
            throw new Exception($"region with id:{id} not found");

            var dto_record = _mapper.Map<RegionDTO>(record);
            return Ok(dto_record);
        }
        [HttpPost]
        [Route("addRegion")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateAsync(RegionDTO dto,CancellationToken token)
        {
            if(!ModelState.IsValid || dto is null)
            { 
                throw new Exception("invalid model");
            }
            var region = _mapper.Map<Region>(dto);
            await _repo.CreateAsync(region);
            await _uow.SaveChangeAsync(token);

            return Ok(region.RegionId);
        }
        [HttpPut]
        [Route("updateRegion")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateAsync(int id, RegionDTO dto, CancellationToken token)
        {
            var record = await _repo.GetById(id);
            if(record is null || id <=0 || !ModelState.IsValid || dto is null)
            throw new Exception($"region with id:{id} not found");

            var region = _mapper.Map<Region>(dto);
            await _repo.UpdateAsync(id,region);
            await _uow.SaveChangeAsync(token);
            return Ok(region.RegionId);
        }
        [HttpDelete]
        [Route("deleteRegion")]
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
            return Ok(record.RegionId);
        }
        [HttpPatch]
        [Route("updatePatch")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdatePatchAsync(int id, [FromBody] JsonPatchDocument<RegionDTO> patch,CancellationToken token)
        {
            var record  = await _repo.GetById(id);
            if(record is null || !ModelState.IsValid)
            throw new Exception($"record  with id:{id} not found or model is invalid");

            var dto_record = _mapper.Map<RegionDTO>(record);
            patch.ApplyTo(dto_record,ModelState);

            var region = _mapper.Map<Region>(dto_record);
            await _repo.UpdateAsync(id,region);
            await _uow.SaveChangeAsync(token);
            
            return Ok(region.RegionId);
        }
    }
}