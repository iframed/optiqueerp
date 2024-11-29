using Microsoft.AspNetCore.Mvc;
using optique.Dtos;
using optique.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefTypeClientController : ControllerBase
    {
        private readonly ITypeClientService _typeClientService;

        public RefTypeClientController(ITypeClientService typeClientService)
        {
            _typeClientService = typeClientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefTypeClientDTO>>> GetAll()
        {
            var types = await _typeClientService.GetAllAsync();
            return Ok(types);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RefTypeClientDTO>> GetById(int id)
        {
            var type = await _typeClientService.GetByIdAsync(id);
            if (type == null)
            {
                return NotFound();
            }
            return Ok(type);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RefTypeClientDTO refTypeClientDTO)
        {
            if (refTypeClientDTO == null)
            {
                return BadRequest();
            }

            await _typeClientService.AddAsync(refTypeClientDTO);
            return CreatedAtAction(nameof(GetById), new { id = refTypeClientDTO.Id }, refTypeClientDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] RefTypeClientDTO refTypeClientDTO)
        {
            if (id != refTypeClientDTO.Id)
            {
                return BadRequest();
            }

            var existingType = await _typeClientService.GetByIdAsync(id);
            if (existingType == null)
            {
                return NotFound();
            }

            await _typeClientService.UpdateAsync(refTypeClientDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingType = await _typeClientService.GetByIdAsync(id);
            if (existingType == null)
            {
                return NotFound();
            }

            await _typeClientService.DeleteAsync(id);
            return NoContent();
        }
    }
}
