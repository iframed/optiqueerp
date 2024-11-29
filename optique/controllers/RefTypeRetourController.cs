using Microsoft.AspNetCore.Mvc;
using optique.Dtos;
using optique.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefTypeRetourController : ControllerBase
    {
        private readonly IRefTypeRetourService _refTypeRetourService;

        public RefTypeRetourController(IRefTypeRetourService refTypeRetourService)
        {
            _refTypeRetourService = refTypeRetourService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefTypeRetourDTO>>> GetAll()
        {
            var refTypeRetours = await _refTypeRetourService.GetAllAsync();
            return Ok(refTypeRetours);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RefTypeRetourDTO>> GetById(int id)
        {
            var refTypeRetour = await _refTypeRetourService.GetByIdAsync(id);
            if (refTypeRetour == null)
            {
                return NotFound();
            }
            return Ok(refTypeRetour);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RefTypeRetourDTO dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            try
            {
                await _refTypeRetourService.AddAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.InnerException as Microsoft.Data.SqlClient.SqlException;
                if (sqlException != null)
                {
                    // Inspectez les détails de l'exception SQL
                    Console.WriteLine(sqlException.Message);
                }
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] RefTypeRetourDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var existingRefTypeRetour = await _refTypeRetourService.GetByIdAsync(id);
            if (existingRefTypeRetour == null)
            {
                return NotFound();
            }

            await _refTypeRetourService.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingRefTypeRetour = await _refTypeRetourService.GetByIdAsync(id);
            if (existingRefTypeRetour == null)
            {
                return NotFound();
            }

            await _refTypeRetourService.DeleteAsync(id);
            return NoContent();
        }
    }
}
