/*using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using optique.Dtos;
using optique.IServices;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VenteController : ControllerBase
    {
        private readonly IVenteService _venteService;
        private readonly ILogger<VenteController> _logger;

        public VenteController(IVenteService venteService, ILogger<VenteController> logger)
        {
            _venteService = venteService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VenteDetailsDTO>>> GetAll()
        {
            _logger.LogDebug("Entering GetAll method.");
            var ventes = await _venteService.GetAllAsync();
            return Ok(ventes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VenteDetailsDTO>> GetById(int id)
        {
            _logger.LogDebug("Entering GetById method with id: {Id}", id);
            var vente = await _venteService.GetByIdAsync(id);
            if (vente == null)
            {
                _logger.LogWarning("Vente with id {Id} not found.", id);
                return NotFound();
            }
            return Ok(vente);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] VenteDTO venteDTO)
        {
            _logger.LogDebug("Entering Add method.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for VenteDTO: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                var userName = User.Identity?.Name;
                _logger.LogDebug("Authenticated User: {UserName}", userName);

                if (string.IsNullOrEmpty(userName))
                {
                    _logger.LogWarning("User is not authenticated or UserName is null.");
                    return Unauthorized("User is not authenticated.");
                }

                venteDTO.UserName = userName;
                _logger.LogDebug("Adding venteDTO: {VenteDTO}", venteDTO);

                await _venteService.AddAsync(venteDTO, userName);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new vente.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] VenteDTO venteDTO)
        {
            _logger.LogDebug("Entering Update method with id: {Id}", id);

            if (id != venteDTO.Id)
            {
                _logger.LogWarning("ID mismatch: URL id {Id} does not match VenteDTO id {VenteDTOId}.", id, venteDTO.Id);
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for VenteDTO: {ModelState}", ModelState);
                return BadRequest(ModelState);
            }

            try
            {
                await _venteService.UpdateAsync(venteDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating vente with id {Id}.", id);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogDebug("Entering Delete method with id: {Id}", id);

            try
            {
                await _venteService.DeleteAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting vente with id {Id}.", id);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("cheques")]
        public async Task<ActionResult<IEnumerable<ChequeDueDateDTO>>> GetChequeDueDates()
        {
            _logger.LogDebug("Entering GetChequeDueDates method.");
            var cheques = await _venteService.GetChequeDueDatesAsync();
            if (cheques == null || !cheques.Any())
            {
                _logger.LogWarning("No cheques found.");
                return NotFound("No cheques found.");
            }
            _logger.LogDebug("Returning {Count} cheques.", cheques.Count());
            return Ok(cheques);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<VenteDetailsDTO>>> Search(
            [FromQuery] string? client, 
            [FromQuery] string? marque, 
            [FromQuery] string? typeArticle, 
            [FromQuery] string? reference)
        {
            _logger.LogDebug("Entering Search method with parameters: client={Client}, marque={Marque}, typeArticle={TypeArticle}, reference={Reference}", client, marque, typeArticle, reference);
            var results = await _venteService.SearchAsync(client, marque, typeArticle, reference);
            return Ok(results);
        }
    }
}
*/