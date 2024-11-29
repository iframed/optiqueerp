using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using optique.Dtos;
using optique.IServices;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RefTypeDePaiementController : ControllerBase
    {
        private readonly IRefTypeDePaiementService _service;

        public RefTypeDePaiementController(IRefTypeDePaiementService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RefTypeDePaiementDTO>>> GetAll()
        {
            var refTypes = await _service.GetAllAsync();
            return Ok(refTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RefTypeDePaiementDTO>> GetById(int id)
        {
            var refType = await _service.GetByIdAsync(id);
            if (refType == null)
            {
                return NotFound();
            }
            return Ok(refType);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] RefTypeDePaiementDTO refTypeDePaiementDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _service.AddAsync(refTypeDePaiementDTO);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] RefTypeDePaiementDTO refTypeDePaiementDTO)
        {
            if (id != refTypeDePaiementDTO.Id)
            {
                return BadRequest("ID mismatch");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _service.UpdateAsync(refTypeDePaiementDTO);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return Ok();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
