using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using northwindAPI.Data.ModelDTO;
using northwindAPI.Models;
using northwindAPI.PatternService.UnitOfWork;
using northwindAPI.RepositoryService;

namespace northwindAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController:ControllerBase
    {
        private readonly IRepository<Supplier> _repo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper; 
        public SupplierController(IRepository<Supplier> repo, IUnitOfWork uow, IMapper mapper)
        {
            _repo = repo;
            _uow = uow;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("getSuppliers")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IQueryable<SupplierDTO>>> GetAllAsync()
        {
            var records = await  _repo.GetAllAsync();
            if(records is null)
            throw new Exception("suppliers not found");
            
            var dto_records= _mapper.Map<List<SupplierDTO>>(records);

            return Ok(dto_records);
        }
        [HttpGet]
        [Route("getSupplierById {id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<SupplierDTO>> GetByIdAsync(int id)
        {
            var record = await  _repo.GetById(id);
            if(record is null || id <=0)
            throw new Exception($"record not found or id {id} is invalid");

            var dto_record = _mapper.Map<SupplierDTO>(record);

            return Ok(dto_record);
        }
        [HttpPost]
        [Route("addSupplier")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> CreateProductAsync(SupplierDTO dto, CancellationToken token)
        {
            if(ModelState.IsValid)
            {
                throw new Exception("invalid model");
            }
            var supplier = _mapper.Map<Supplier>(dto);

            await _repo.CreateAsync(supplier);
            await _uow.SaveChangeAsync(token);

            return Ok();
        }
        [HttpPut]
        [Route("updateSupplier")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> UpdateAsync(int id, SupplierDTO dto,CancellationToken token)
        {
            var record = GetByIdAsync(id);
            if(record is null || id <=0)
            throw new Exception($"record not found or id {id} is invalid");

            var supplier = _mapper.Map<Supplier>(dto);
            await _repo.UpdateAsync(id,supplier);
            await _uow.SaveChangeAsync(token);

            return Ok();
        }
        [HttpDelete]
        [Route("deleteSupplier")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult> DeleteAsync(int id,CancellationToken token)
        {
            var record = GetByIdAsync(id);
            if(record is null)
            throw new Exception($"supplier {id} not found");

            await _repo.DeleteAsync(id);
            await _uow.SaveChangeAsync(token);

            return Ok();
        } 
    }
}