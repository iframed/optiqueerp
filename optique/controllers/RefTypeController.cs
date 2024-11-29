using Microsoft.AspNetCore.Mvc;
using optique.IServices;
using optique.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RefTypeController : ControllerBase
    {
        private readonly IRefTypeService _refTypeService;

        public RefTypeController(IRefTypeService refTypeService)
        {
            _refTypeService = refTypeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefTypeDTO>>> GetAll()
        {
            var refTypes = await _refTypeService.GetAllAsync();
            return Ok(refTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RefTypeDTO>> GetById(int id)
        {
            var refType = await _refTypeService.GetByIdAsync(id);
            if (refType == null)
            {
                return NotFound();
            }
            return Ok(refType);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RefTypeDTO refTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _refTypeService.AddAsync(refTypeDTO);
            return CreatedAtAction(nameof(GetById), new { id = refTypeDTO.Id }, refTypeDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] RefTypeDTO refTypeDTO)
        {
            if (id != refTypeDTO.Id)
            {
                return BadRequest("ID mismatch");
            }

            try
            {
                await _refTypeService.UpdateAsync(refTypeDTO);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _refTypeService.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }
    }
}
