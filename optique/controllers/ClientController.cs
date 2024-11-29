using Microsoft.AspNetCore.Mvc;
using optique.IServices;
using optique.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetAll()
        {
            var clients = await _clientService.GetAllAsync();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDTO>> GetById(int id)
        {
            var client = await _clientService.GetByIdAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return Ok(client);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ClientDTO clientDTO)
        {
            if (clientDTO == null)
            {
                return BadRequest();
            }

            await _clientService.AddAsync(clientDTO);
            return CreatedAtAction(nameof(GetById), new { id = clientDTO.Id }, clientDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ClientDTO clientDTO)
        {
            if (id != clientDTO.Id)
            {
                return BadRequest();
            }

            var existingClient = await _clientService.GetByIdAsync(id);
            if (existingClient == null)
            {
                return NotFound();
            }

            await _clientService.UpdateAsync(clientDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingClient = await _clientService.GetByIdAsync(id);
            if (existingClient == null)
            {
                return NotFound();
            }

            await _clientService.DeleteAsync(id);
            return NoContent();
        }


        [HttpGet("type/libelle/{typeLibelle}")]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetByTypeLibelle(string typeLibelle)
        {
            var clients = await _clientService.GetByTypeLibelleAsync(typeLibelle);
            return Ok(clients);
        }

        [HttpGet("adresse/{adresse}")]
        public async Task<ActionResult<IEnumerable<ClientDTO>>> GetByAdresse(string adresse)
        {
            var clients = await _clientService.GetByAdresseAsync(adresse);
            return Ok(clients);
        }
    }
}
