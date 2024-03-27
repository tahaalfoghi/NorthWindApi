using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class ProductController:ControllerBase
    {
        private readonly IRepository<Product> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public ProductController(IRepository<Product> repo, IUnitOfWork uow, IMapper mapper)
        {
            _repo = repo;
            _uow = uow;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("getProducts")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IQueryable<ProductDTO>>> GetAllAsync()
        {
            var records = await  _repo.GetAllAsync();
            if(records is null)
            throw new Exception("products not found");
            
            var dto_records= _mapper.Map<List<ProductDTO>>(records);

            return Ok(dto_records);
        }
        [HttpGet]
        [Route("getProductById {id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<ProductDTO>> GetByIdAsync(int id)
        {
            var record = await  _repo.GetById(id);
            if(record is null || id <=0)
            throw new Exception($"record not found or id {id} is invalid");

            var dto_record = _mapper.Map<ProductDTO>(record);

            return Ok(dto_record);
        }
        [HttpPost]
        [Route("addProduct")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateProductAsync(ProductDTO dto, CancellationToken token)
        {
            if(ModelState.IsValid)
            {
                throw new Exception("invalid model");
            }
            var product = _mapper.Map<Product>(dto);

            await _repo.CreateAsync(product);
            await _uow.SaveChangeAsync(token);

            return Ok();
        }
        [HttpPut]
        [Route("updateProduct")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateAsync(int id, ProductDTO dto,CancellationToken token)
        {
            var record = GetByIdAsync(id);
            if(record is null || id <=0)
            throw new Exception($"record  not found or id {id} is invalid");

            var product = _mapper.Map<Product>(dto);
            await _repo.UpdateAsync(id,product);
            await _uow.SaveChangeAsync(token);

            return Ok();
        }
        [HttpDelete]
        [Route("deleteProduct")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteAsync(int id,CancellationToken token)
        {
            var record = GetByIdAsync(id);
            if(record is null)
            throw new Exception($"product {id} not found");

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
        public async Task<ActionResult> UpdatePatchAsync(int id, [FromBody] JsonPatchDocument<ProductDTO> patch,CancellationToken token)
        {
            var record  = await _repo.GetById(id);
            if(record is null || !ModelState.IsValid)
            throw new Exception($"record  with id:{id} not found or model is invalid");

            var dto_record = _mapper.Map<ProductDTO>(record);
            patch.ApplyTo(dto_record,ModelState);

            var product = _mapper.Map<Product>(dto_record);
            await _repo.UpdateAsync(id,product);
            await _uow.SaveChangeAsync(token);
            
            return Ok(product.ProductId);
        }
    }
}