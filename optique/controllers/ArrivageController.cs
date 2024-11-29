using Microsoft.AspNetCore.Mvc;
using optique.Data;
using optique.IServices;
using optique.Dtos;
using optique.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArrivageController : ControllerBase
    {
        private readonly IArrivageService _arrivageService;
        private readonly ApplicationDbContext _context; 

        public ArrivageController(IArrivageService arrivageService, ApplicationDbContext context)
        {
            _arrivageService = arrivageService;
            _context = context; // Assignation du contexte injecté
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArrivageDTO>>> GetAll()
        {
            var arrivages = await _arrivageService.GetAllAsync();
            return Ok(arrivages);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArrivageDTO>> GetById(int id)
        {
            var arrivage = await _arrivageService.GetByIdAsync(id);
            if (arrivage == null)
            {
                return NotFound();
            }
            return Ok(arrivage);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ArrivageDTO dto)
        {
            if (dto == null)
            {
                return BadRequest();
            }

            var newArrivage = new Arrivage
            {
                DateArrivage = DateTime.Now,
                SocieteId = dto.SocieteId, 
                FournisseurId = dto.FournisseurId,
                NumBL = dto.NumBL,
                NumFacture = dto.NumFacture,
                MontantFacture = dto.MontantFacture,
                //DeviseId = dto.DeviseId 
            };

            try
            {
                _context.Arrivages.Add(newArrivage);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = newArrivage.Id }, newArrivage);
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
        public async Task<ActionResult> Update(int id, [FromBody] ArrivageDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var existingArrivage = await _arrivageService.GetByIdAsync(id);
            if (existingArrivage == null)
            {
                return NotFound();
            }

            await _arrivageService.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingArrivage = await _arrivageService.GetByIdAsync(id);
            if (existingArrivage == null)
            {
                return NotFound();
            }

         
            await _arrivageService.DeleteAsync(id);
            return NoContent();
        }
    }
}
