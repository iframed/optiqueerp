using Microsoft.AspNetCore.Mvc;
using optique.IServices;
using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefMarqueController : ControllerBase
    {
        private readonly IRefMarqueService _refMarqueService;

        public RefMarqueController(IRefMarqueService refMarqueService)
        {
            _refMarqueService = refMarqueService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefMarqueDTO>>> GetAll()
        {
            var refMarques = await _refMarqueService.GetAllAsync();
            return Ok(refMarques);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RefMarqueDTO>> GetById(int id)
        {
            var refMarque = await _refMarqueService.GetByIdAsync(id);
            if (refMarque == null)
            {
                return NotFound();
            }
            return Ok(refMarque);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RefMarqueDTO refMarqueDTO)
        {
            if (refMarqueDTO == null)
            {
                return BadRequest();
            }

            await _refMarqueService.AddAsync(refMarqueDTO);
            return CreatedAtAction(nameof(GetById), new { id = refMarqueDTO.Id }, refMarqueDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] RefMarqueDTO refMarqueDTO)
        {
            if (id != refMarqueDTO.Id)
            {
                return BadRequest();
            }

            var existingRefMarque = await _refMarqueService.GetByIdAsync(id);
            if (existingRefMarque == null)
            {
                return NotFound();
            }

            await _refMarqueService.UpdateAsync(refMarqueDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingRefMarque = await _refMarqueService.GetByIdAsync(id);
            if (existingRefMarque == null)
            {
                return NotFound();
            }

            await _refMarqueService.DeleteAsync(id);
            return NoContent();
        }
    }
}
