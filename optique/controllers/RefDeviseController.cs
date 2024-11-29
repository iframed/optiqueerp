using Microsoft.AspNetCore.Mvc;
using optique.IServices;
using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefDeviseController : ControllerBase
    {
        private readonly IRefDeviseService _refDeviseService;

        public RefDeviseController(IRefDeviseService refDeviseService)
        {
            _refDeviseService = refDeviseService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefDeviseDTO>>> GetAll()
        {
            var refDevises = await _refDeviseService.GetAllAsync();
            return Ok(refDevises);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RefDeviseDTO>> GetById(int id)
        {
            var refDevise = await _refDeviseService.GetByIdAsync(id);
            if (refDevise == null)
            {
                return NotFound();
            }
            return Ok(refDevise);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RefDeviseDTO refDeviseDTO)
        {
            if (refDeviseDTO == null)
            {
                return BadRequest();
            }

            await _refDeviseService.AddAsync(refDeviseDTO);
            return CreatedAtAction(nameof(GetById), new { id = refDeviseDTO.Id }, refDeviseDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] RefDeviseDTO refDeviseDTO)
        {
            if (id != refDeviseDTO.Id)
            {
                return BadRequest();
            }

            var existingRefDevise = await _refDeviseService.GetByIdAsync(id);
            if (existingRefDevise == null)
            {
                return NotFound();
            }

            await _refDeviseService.UpdateAsync(refDeviseDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingRefDevise = await _refDeviseService.GetByIdAsync(id);
            if (existingRefDevise == null)
            {
                return NotFound();
            }

            await _refDeviseService.DeleteAsync(id);
            return NoContent();
        }
    }
}
