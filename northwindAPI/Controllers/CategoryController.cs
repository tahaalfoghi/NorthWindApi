using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using northwindAPI.Data.ModelDTO;
using northwindAPI.Models;
using northwindAPI.PatternService.UnitOfWork;
using northwindAPI.RepositoryService;
using Azure;
using Microsoft.AspNetCore.JsonPatch;

namespace northwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController:ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;
        private readonly IRepository<Category> _repo;

        public CategoryController(IUnitOfWork uow, IMapper mapper, ILogger<CategoryController> logger, IRepository<Category> repo)
        {
            _uow = uow;
            _mapper = mapper;
            _logger = logger;
            _repo = repo;
        }
        [HttpGet]
        [Route("GetAll")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllAsync()
        {
            var records = await _repo.GetAllAsync();
            if(records is null)
            {
                throw new Exception("Not Found");
            }
            
            var dto_records = _mapper.Map<List<CategoryDTO>>(records);
            return dto_records;
        }
        [HttpGet]
        [Route("GetById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var record = await _repo.GetById(id);
            if(record is null)
            {
                throw new Exception("record not found");
            }
            var dto_record = _mapper.Map<CategoryDTO>(record);

            return Ok(dto_record);
        }
        [HttpPost]
        [Route("AddCategory")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateAsync(CategoryDTO dto,CancellationToken token)
        {
            if(!ModelState.IsValid)
            {
                throw new Exception("Invalid data");
            }

            if(dto is null)
            throw new Exception("operation failed invalid data");

            var record = _mapper.Map<Category>(dto);
            await _repo.CreateAsync(record);
            await _uow.SaveChangeAsync(token);
            return Ok();
        }
        [HttpPut]
        [Route("Update Category")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateAsync(int id, CategoryDTO dto,CancellationToken token)
        {
            var record = await _repo.GetById(id);
            if(record is null)
            {
                throw new Exception($"Invalid id {id} not found");
            }
            
            var category = _mapper.Map<Category>(dto);
            await _repo.UpdateAsync(id,category);
            await _uow.SaveChangeAsync(token);

            return Ok();
        }
        [HttpDelete]
        [Route("DeleteById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteAsync(int id, CancellationToken token)
        {
            var record =  await _repo.GetById(id);
            if(record is null)
            {
                throw new Exception($"record not found invalid id {id} value");
            }
            await _repo.DeleteAsync(id);
            await _uow.SaveChangeAsync(token);
            return Ok(record.CategoryId);
        }
        [HttpPatch]
        [Route("updatePatch")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdatePatchAsync(int id, [FromBody] JsonPatchDocument<CategoryDTO> patch,CancellationToken token)
        {
            var record  = await _repo.GetById(id);
            if(record is null || !ModelState.IsValid)
            throw new Exception($"record  with id:{id} not found or model is invalid");

            var dto_record = _mapper.Map<CategoryDTO>(record);
            patch.ApplyTo(dto_record,ModelState);

            var category = _mapper.Map<Category>(dto_record);
            await _repo.UpdateAsync(id,category);
            await _uow.SaveChangeAsync(token);
            
            return Ok(category.CategoryId);
        }
    }
}