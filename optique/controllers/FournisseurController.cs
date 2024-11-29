using Microsoft.AspNetCore.Mvc;
using optique.IServices;
using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FournisseurController : ControllerBase
    {
        private readonly IFournisseurService _fournisseurService;

        public FournisseurController(IFournisseurService fournisseurService)
        {
            _fournisseurService = fournisseurService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FournisseurDTO>>> GetAll()
        {
            var fournisseurs = await _fournisseurService.GetAllAsync();
            return Ok(fournisseurs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FournisseurDTO>> GetById(int id)
        {
            var fournisseur = await _fournisseurService.GetByIdAsync(id);
            if (fournisseur == null)
            {
                return NotFound();
            }
            return Ok(fournisseur);
        }

        /*[HttpPost]
        public async Task<ActionResult> Create([FromBody] FournisseurDTO fournisseurDTO)
        {
            if (fournisseurDTO == null)
            {
                return BadRequest();
            }

            await _fournisseurService.AddAsync(fournisseurDTO);
            return CreatedAtAction(nameof(GetById), new { id = fournisseurDTO.Id }, fournisseurDTO);
        }*/

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] FournisseurDTO fournisseurDTO)
        {
            if (id != fournisseurDTO.Id)
            {
                return BadRequest();
            }

            var existingFournisseur = await _fournisseurService.GetByIdAsync(id);
            if (existingFournisseur == null)
            {
                return NotFound();
            }

            await _fournisseurService.UpdateAsync(fournisseurDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingFournisseur = await _fournisseurService.GetByIdAsync(id);
            if (existingFournisseur == null)
            {
                return NotFound();
            }

            await _fournisseurService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("byDevise/{deviseLibelle}")]
public async Task<ActionResult<IEnumerable<FournisseurDTO>>> GetByDeviseLibelle(string deviseLibelle)
{
    var fournisseurs = await _fournisseurService.GetByDeviseLibelleAsync(deviseLibelle);
    return Ok(fournisseurs);
}

[HttpGet("ice/{ice}")]
        public async Task<ActionResult<FournisseurDTO>> GetByICE(string ice)
        {
            var fournisseur = await _fournisseurService.GetByICEAsync(ice);
            if (fournisseur == null)
            {
                return NotFound();
            }
            return Ok(fournisseur);
        }


[HttpGet("search")]
public async Task<ActionResult<IEnumerable<FournisseurDTO>>> GetByICEAndDeviseLibelle(string ice, string deviseLibelle)
{
    var fournisseurs = await _fournisseurService.GetByICEAndDeviseLibelleAsync(ice, deviseLibelle);
    return Ok(fournisseurs);
}


[HttpPost]
public async Task<ActionResult> Create([FromBody] FournisseurDTO fournisseurDTO)
{
    if (fournisseurDTO == null)
    {
        return BadRequest();
    }

    await _fournisseurService.AddAsync(fournisseurDTO);
    return CreatedAtAction(nameof(GetById), new { id = fournisseurDTO.Id }, fournisseurDTO);
}

        


    }
}
