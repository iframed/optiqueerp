using Microsoft.AspNetCore.Mvc;
using optique.Dtos;
using optique.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefStatutDistributionController : ControllerBase
    {
        private readonly IRefStatutDistributionService _refStatutDistributionService;

        public RefStatutDistributionController(IRefStatutDistributionService refStatutDistributionService)
        {
            _refStatutDistributionService = refStatutDistributionService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefStatutDistributionDTO>>> GetAll()
        {
            var refStatutDistributions = await _refStatutDistributionService.GetAllAsync();
            return Ok(refStatutDistributions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RefStatutDistributionDTO>> GetById(int id)
        {
            var refStatutDistribution = await _refStatutDistributionService.GetByIdAsync(id);
            if (refStatutDistribution == null)
            {
                return NotFound();
            }
            return Ok(refStatutDistribution);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RefStatutDistributionDTO refStatutDistributionDTO)
        {
            if (refStatutDistributionDTO == null)
            {
                return BadRequest();
            }

            try
            {
                await _refStatutDistributionService.AddAsync(refStatutDistributionDTO);
                return CreatedAtAction(nameof(GetById), new { id = refStatutDistributionDTO.Id }, refStatutDistributionDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] RefStatutDistributionDTO refStatutDistributionDTO)
        {
            if (id != refStatutDistributionDTO.Id)
            {
                return BadRequest();
            }

            try
            {
                await _refStatutDistributionService.UpdateAsync(refStatutDistributionDTO);
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
                await _refStatutDistributionService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
