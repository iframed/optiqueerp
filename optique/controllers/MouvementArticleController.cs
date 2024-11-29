using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using optique.Dtos;
using optique.IServices;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MouvementArticleController : ControllerBase
    {
        private readonly IMouvementArticleService _mouvementArticleService;

        public MouvementArticleController(IMouvementArticleService mouvementArticleService)
        {
            _mouvementArticleService = mouvementArticleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MouvementArticleDTO>>> GetAll()
        {
            var mouvements = await _mouvementArticleService.GetAllAsync();
            return Ok(mouvements);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MouvementArticleDTO>> GetById(int id)
        {
            var mouvement = await _mouvementArticleService.GetByIdAsync(id);
            if (mouvement == null)
            {
                return NotFound();
            }
            return Ok(mouvement);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] MouvementArticleDTO mouvementArticleDTO)
        {
            if (mouvementArticleDTO == null)
            {
                return BadRequest();
            }

            try
            {
                await _mouvementArticleService.AddAsync(mouvementArticleDTO);
                return CreatedAtAction(nameof(GetById), new { id = mouvementArticleDTO.Id }, mouvementArticleDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }
    }
}
