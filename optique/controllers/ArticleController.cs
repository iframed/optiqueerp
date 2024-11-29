using Microsoft.AspNetCore.Mvc;
using optique.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using optique.Dtos;
using Microsoft.AspNetCore.Authorization;
using optique.ViewModels;


namespace optique.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        


        public ArticleController(
        IArticleService articleService
        )
    {
        _articleService = articleService;
        
    }


       


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetAll()
        {
            var articles = await _articleService.GetAllAsync();
            return Ok(articles);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleDTO>> GetById(int id)
        {
            var article = await _articleService.GetByIdAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }

        [HttpGet("reference/{reference}")]
        public async Task<ActionResult<ArticleDTO>> GetByReference(string reference)
        {
            var article = await _articleService.GetByReferenceAsync(reference);
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] ArticleDTO articleDTO)
        {
            if (articleDTO == null)
            {
                return BadRequest();
            }

            await _articleService.AddAsync(articleDTO);
            return CreatedAtAction(nameof(GetById), new { id = articleDTO.Id }, articleDTO);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] ArticleDTO articleDTO)
        {
            if (id != articleDTO.Id)
            {
                return BadRequest();
            }

            var existingArticle = await _articleService.GetByIdAsync(id);
            if (existingArticle == null)
            {
                return NotFound();
            }

            await _articleService.UpdateAsync(articleDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existingArticle = await _articleService.GetByIdAsync(id);
            if (existingArticle == null)
            {
                return NotFound();
            }

            await _articleService.DeleteAsync(id);
            return NoContent();
        }


        [HttpGet("marque/{libelle}")]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetByMarqueLibelle(string libelle) // Nouvelle méthode
        {
            var articles = await _articleService.GetByMarqueLibelleAsync(libelle);
            return Ok(articles);
        }

        [HttpGet("type/{libelle}")]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetByTypeLibelle(string libelle) 
        {
            var articles = await _articleService.GetByTypeLibelleAsync(libelle);
            return Ok(articles);
        }

        [HttpGet("fournisseur/adresse/{adresse}")]
        public async Task<ActionResult<IEnumerable<ArticleDTO>>> GetByFournisseurAdresse(string adresse)
        {
            var articles = await _articleService.GetByFournisseurAdresseAsync(adresse);
            return Ok(articles);
        }


        [HttpGet("detail")]
        public async Task<ActionResult<IEnumerable<ArticleDetailsDTO>>> GetArticleDetails()
        {
            var articles = await _articleService.GetArticleDetailsAsync();
            return Ok(articles);
        }



        [HttpGet("search")]
public async Task<ActionResult<IEnumerable<ArticleDetailsDTO>>> Search([FromQuery] string? societe, [FromQuery] string? fournisseur, [FromQuery] string? marque, [FromQuery] string? reference)
{
    var articles = await _articleService.SearchArticleDetailsByCriteriaAsync(societe, fournisseur, marque, reference);
    return Ok(articles);
}


  [HttpGet("search-articles")]
public async Task<ActionResult<IEnumerable<ArticleDTO>>> SearchArticles(
    [FromQuery] string? fournisseur, 
    [FromQuery] string? marque, 
    [FromQuery] string? type, 
    [FromQuery] string? reference)
{
    var articles = await _articleService.SearchArticlesByCriteriaAsync(fournisseur, marque, type, reference);
    return Ok(articles);
}


   
}
}
