using Microsoft.AspNetCore.Mvc;
using optique.IServices;
using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SocieteController : ControllerBase
    {
        private readonly ISocieteService _societeService;

        public SocieteController(ISocieteService societeService)
        {
            _societeService = societeService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SocieteDTO>>> GetAll()
        {
            var societes = await _societeService.GetAllAsync();
            return Ok(societes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SocieteDTO>> GetById(int id)
        {
            var societe = await _societeService.GetByIdAsync(id);
            if (societe == null)
            {
                return NotFound();
            }
            return Ok(societe);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] SocieteDTO societeDTO)
        {
            if (societeDTO == null)
            {
                return BadRequest();
            }

            await _societeService.AddAsync(societeDTO);
            return CreatedAtAction(nameof(GetById), new { id = societeDTO.Id }, societeDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] SocieteDTO societeDTO)
        {
            if (id != societeDTO.Id)
            {
                return BadRequest();
            }

            var existingSociete = await _societeService.GetByIdAsync(id);
            if (existingSociete == null)
            {
                return NotFound();
            }

            await _societeService.UpdateAsync(societeDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingSociete = await _societeService.GetByIdAsync(id);
            if (existingSociete == null)
            {
                return NotFound();
            }

            await _societeService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("name/{nomSociete}")]
        public async Task<ActionResult<SocieteDTO>> GetByName(string nomSociete)
        {
            var societe = await _societeService.GetByNameAsync(nomSociete);
            if (societe == null)
            {
                return NotFound();
            }
            return Ok(societe);
        }

        [HttpGet("adresse/{adresse}")]
        public async Task<ActionResult<IEnumerable<SocieteDTO>>> GetByAdresse(string adresse)
        {
            var societes = await _societeService.GetByAdresseAsync(adresse);
            return Ok(societes);
        }
    }
}
