using Microsoft.AspNetCore.Mvc;
using optique.IServices;
using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RetourFournisseurController : ControllerBase
    {
        private readonly IRetourFournisseurService _retourFournisseurService;
        private readonly ILogger<RetourFournisseurController> _logger;

        public RetourFournisseurController(IRetourFournisseurService retourFournisseurService , ILogger<RetourFournisseurController> logger)
        {
            _retourFournisseurService = retourFournisseurService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RetourFournisseurDTO>>> GetAll()
        {
            var retourFournisseurs = await _retourFournisseurService.GetAllAsync();
            return Ok(retourFournisseurs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RetourFournisseurDTO>> GetById(int id)
        {
            var retourFournisseur = await _retourFournisseurService.GetByIdAsync(id);
            if (retourFournisseur == null)
            {
                return NotFound();
            }
            return Ok(retourFournisseur);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RetourFournisseurDTO retourFournisseurDTO)
        {
            if (retourFournisseurDTO == null)
            {
                return BadRequest();
            }

            try
            {
                var userName = User.Identity?.Name;
                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not authenticated.");
                }

                retourFournisseurDTO.UserName = userName;

                await _retourFournisseurService.AddAsync(retourFournisseurDTO, userName);
                return CreatedAtAction(nameof(GetById), new { id = retourFournisseurDTO.Id }, retourFournisseurDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new retour fournisseur.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] RetourFournisseurDTO retourFournisseurDTO)
        {
            if (id != retourFournisseurDTO.Id)
            {
                return BadRequest();
            }

            await _retourFournisseurService.UpdateAsync(retourFournisseurDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _retourFournisseurService.DeleteAsync(id);
            return NoContent();
        }


[HttpGet("details")]
        public async Task<ActionResult<IEnumerable<FournisseurDetailsDTO>>> GetFournisseurDetails()
        {
            var details = await _retourFournisseurService.GetFournisseurDetailsAsync();
            return Ok(details);
        }

    [HttpGet("search")]
public async Task<ActionResult<IEnumerable<FournisseurDetailsDTO>>> GetFournisseurDetailsByCriteriaAsync([FromQuery] string? societe, [FromQuery] string? marque, [FromQuery] string? reference, [FromQuery] string?  fournisseur)
{
    var details = await _retourFournisseurService.GetFournisseurDetailsByCriteriaAsync(societe, marque, reference, fournisseur);
    return Ok(details);
}


        


    }
}
