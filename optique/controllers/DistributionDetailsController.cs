using Microsoft.AspNetCore.Mvc;
using optique.Dtos;
using optique.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DistributionDetailsController : ControllerBase
    {
        private readonly IDistributionDetailsService _distributionDetailsService;

         private readonly ILogger<DistributionDetailsController> _logger;

        public DistributionDetailsController(IDistributionDetailsService distributionDetailsService , ILogger<DistributionDetailsController> logger)
        {
            _distributionDetailsService = distributionDetailsService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DistributionDetailsDTO>>> GetAll()
        {
            var distributionDetails = await _distributionDetailsService.GetAllAsync();
            return Ok(distributionDetails);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DistributionDetailsDTO>> GetById(int id)
        {
            var distributionDetails = await _distributionDetailsService.GetByIdAsync(id);
            if (distributionDetails == null)
            {
                return NotFound();
            }
            return Ok(distributionDetails);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] DistributionDetailsDTO distributionDetailsDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userName = User.Identity?.Name;

                if (string.IsNullOrEmpty(userName))
                {
                    return Unauthorized("User is not authenticated.");
                }

                await _distributionDetailsService.AddAsync(distributionDetailsDTO, userName);
                return CreatedAtAction(nameof(GetById), new { id = distributionDetailsDTO.Id }, distributionDetailsDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new distribution detail.");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] DistributionDetailsDTO distributionDetailsDTO)
        {
            if (id != distributionDetailsDTO.Id)
            {
                return BadRequest();
            }

            try
            {
                await _distributionDetailsService.UpdateAsync(distributionDetailsDTO);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _distributionDetailsService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
