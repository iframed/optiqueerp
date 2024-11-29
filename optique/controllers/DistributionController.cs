using Microsoft.AspNetCore.Mvc;
using optique.Dtos;
using optique.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using optique.Data;
using AutoMapper;
using optique.Models;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DistributionController : ControllerBase
    {
        private readonly IDistributionService _distributionService;
         private readonly ApplicationDbContext _context;  
        private readonly IMapper _mapper; 

        public DistributionController(IDistributionService distributionService , ApplicationDbContext context, IMapper mapper)
        {
            _distributionService = distributionService;
             _context = context;  
            _mapper = mapper; 
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DistributionDTO>>> GetAll()
        {
            var distributions = await _distributionService.GetAllAsync();
            return Ok(distributions);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DistributionDTO>> GetById(int id)
        {
            var distribution = await _distributionService.GetByIdAsync(id);
            if (distribution == null)
            {
                return NotFound();
            }
            return Ok(distribution);
        }

        /*[HttpPost]
        public async Task<ActionResult> Create([FromBody] DistributionDTO distributionDTO)
        {
            if (distributionDTO == null)
            {
                return BadRequest();
            }

            try
            {
                await _distributionService.AddAsync(distributionDTO);
                return CreatedAtAction(nameof(GetById), new { id = distributionDTO.Id }, distributionDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }*/
           [HttpPost]
        public async Task<ActionResult<DistributionDTO>> Create(DistributionDTO distributionDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var distribution = _mapper.Map<Distribution>(distributionDTO);

            _context.Distributions.Add(distribution);
            await _context.SaveChangesAsync();

            distributionDTO.Id = distribution.Id; // Make sure ID is set here

            return CreatedAtAction(nameof(GetById), new { id = distribution.Id }, distributionDTO);
        }




        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] DistributionDTO distributionDTO)
        {
            if (id != distributionDTO.Id)
            {
                return BadRequest();
            }

            try
            {
                await _distributionService.UpdateAsync(distributionDTO);
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
                await _distributionService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpGet("summary")]
        public async Task<ActionResult<IEnumerable<DistributionSummaryDTO>>> GetDistributionSummary()
        {
            var result = await _distributionService.GetDistributionSummaryAsync();
            return Ok(result);
        }


        [HttpGet("summaryByClient/{clientId}")]
    public async Task<ActionResult<IEnumerable<DistributionSummaryDTO>>> GetDistributionSummaryByClient(int clientId)
    {
        var result = await _distributionService.GetDistributionSummaryByClientAsync(clientId);
        return Ok(result);
    }



    [HttpGet("summaryByFournisseur/{fournisseurId}")]
    public async Task<ActionResult<IEnumerable<DistributionSummaryDTO>>> GetDistributionSummaryByFournisseur(int fournisseurId)
    {
        var result = await _distributionService.GetDistributionSummaryByFournisseurAsync(fournisseurId);
        return Ok(result);
    }


    [HttpGet("marque/{marqueLibelle}")]
    public async Task<ActionResult<IEnumerable<DistributionSummaryDTO>>> GetDistributionSummaryByMarqueLibelle(string marqueLibelle)
    {
        var result = await _distributionService.GetDistributionSummaryByMarqueLibelleAsync(marqueLibelle);
        return Ok(result);
    }


    [HttpGet("statut/{statutLibelle}")]
    public async Task<ActionResult<IEnumerable<DistributionSummaryDTO>>> GetDistributionSummaryByStatutLibelle(string statutLibelle)
    {
        var result = await _distributionService.GetDistributionSummaryByStatutLibelleAsync(statutLibelle);
        return Ok(result);
    }



    [HttpGet("reference/{reference}")]
    public async Task<ActionResult<IEnumerable<DistributionSummaryDTO>>> GetDistributionSummaryByReference(string reference)
    {
        var result = await _distributionService.GetDistributionSummaryByReferenceAsync(reference);
        return Ok(result);
    }



    [HttpGet("search")]
public async Task<ActionResult<IEnumerable<DistributionSummaryDTO>>> Search([FromQuery] string? client, [FromQuery] string? fournisseur, [FromQuery] string? marque, [FromQuery] string? statut, [FromQuery] string? reference)
{
    var result = await _distributionService.SearchByCriteriaAsync(client, fournisseur, marque, statut, reference);
    return Ok(result);
}

    }
}
