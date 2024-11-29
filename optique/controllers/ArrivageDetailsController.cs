using Microsoft.AspNetCore.Mvc;
using optique.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using optique.Dtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ArrivageDetailsController : ControllerBase
    {
        private readonly IArrivageDetailsService _arrivageDetailsService;
        private readonly ILogger<ArrivageDetailsController> _logger;


        public ArrivageDetailsController(IArrivageDetailsService arrivageDetailsService , ILogger<ArrivageDetailsController> logger)
        {
            _arrivageDetailsService = arrivageDetailsService;
             _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArrivageDetailsDTO>>> GetAll()
        {
            var details = await _arrivageDetailsService.GetAllAsync();
            return Ok(details);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArrivageDetailsDTO>> GetById(int id)
        {
            var detail = await _arrivageDetailsService.GetByIdAsync(id);
            if (detail == null)
            {
                return NotFound();
            }
            return Ok(detail);
        }

       

       [HttpPost]
        public async Task<ActionResult> Add([FromBody] ArrivageDetailsDTO arrivageDetailsDTO)
        {
            _logger.LogDebug("Entering Add method.");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for ArrivageDetailsDTO: {ModelState}", ModelState);
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

                var claimsIdentity = User.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    foreach (var claim in claimsIdentity.Claims)
                    {
                        _logger.LogDebug("Claim Type: {ClaimType}, Claim Value: {ClaimValue}", claim.Type, claim.Value);
                    }
                }

                // Assigner le nom d'utilisateur connecté au ArrivageDetailsDTO
                arrivageDetailsDTO.UserName = userName;

                _logger.LogDebug("Adding arrivageDetailsDTO: {ArrivageDetailsDTO}", arrivageDetailsDTO);

                await _arrivageDetailsService.AddAsync(arrivageDetailsDTO, userName);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new arrivage detail.");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ArrivageDetailsDTO dto)
        {
            if (id != dto.Id)
            {
                return BadRequest();
            }

            var existingDetail = await _arrivageDetailsService.GetByIdAsync(id);
            if (existingDetail == null)
            {
                return NotFound();
            }

            await _arrivageDetailsService.UpdateAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingDetail = await _arrivageDetailsService.GetByIdAsync(id);
            if (existingDetail == null)
            {
                return NotFound();
            }

            await _arrivageDetailsService.DeleteAsync(id);
            return NoContent();
        }
    }
}
