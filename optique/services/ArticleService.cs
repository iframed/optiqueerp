using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyAspNetApp.Repositories;
using optique.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using optique.Data;
using optique.Dtos;
using optique.Models;
using optique.Specifications;
using Microsoft.AspNetCore.Mvc;
using optique.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace optique.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IRepository<Article> _repository;
        private readonly IMapper _mapper;

        private readonly ApplicationDbContext _context;

         private readonly IRefMarqueService _marqueService;
        private readonly IRefTypeService _typeService;
        private readonly IFournisseurService _fournisseurService;
        private readonly ILogger<ArticleService> _logger;


        public ArticleService(IRepository<Article> repository, IMapper mapper,ApplicationDbContext context,IRefMarqueService marqueService, 
            IRefTypeService typeService, 
            IFournisseurService fournisseurService,ILogger<ArticleService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
            _marqueService = marqueService;
            _typeService = typeService;
            _fournisseurService = fournisseurService;
                _logger = logger;  
        }

        


       /*public async Task<IEnumerable<ArticleDTO>> GetAllAsync()
        {
            var articles = await _repository.ListAsync();
            return _mapper.Map<IEnumerable<ArticleDTO>>(articles);
        }*/

        public async Task<IEnumerable<ArticleDTO>> GetAllAsync()
{
    var articles = await _context.Articles
                                .Where(a => !a.IsDeleted) // Ne retourne que les articles non supprimés
                                .ToListAsync();
    return _mapper.Map<IEnumerable<ArticleDTO>>(articles);
}

public async Task DeleteAsync(int id)
{
    var article = await _context.Articles.FindAsync(id);
    if (article != null)
    {
        article.IsDeleted = true;  // Marquez l'article comme supprimé
        _context.Articles.Update(article);
        await _context.SaveChangesAsync();
    }
}


  

        public async Task<ArticleDTO?> GetByIdAsync(int id)
    {
        var article = await _repository.GetByIdAsync(id, query => query.Include(a => a.Marque));
       
    
        return _mapper.Map<ArticleDTO?>(article);  // Assurez-vous d'utiliser votre logique de mappage
    }

    /*public async Task<ArticleDTO?> GetByIdAsync(int id)
{
    // Inclure le filtre IsDeleted et la relation Marque
    var article = await _repository.GetByIdAsync(id, query => query
        .Where(a => !a.IsDeleted) // Filtrer les articles non supprimés
        .Include(a => a.Marque)); // Inclure les informations de Marque
    
    return _mapper.Map<ArticleDTO?>(article);  // Assurez-vous d'utiliser votre logique de mappage
}*/





        public async Task<ArticleDTO?> GetByReferenceAsync(string reference)
        {
            var spec = new ArticleByReferenceSpecification(reference);
            var article = await _repository.FirstOrDefaultAsync(spec);
            return _mapper.Map<ArticleDTO?>(article);
        }

       
        
        


        public async Task UpdateAsync(ArticleDTO articleDTO)
        {
            var article = await _repository.GetByIdAsync(articleDTO.Id);
            if (article == null)
            {
                throw new Exception("L'article spécifié n'existe pas.");
            }

            _mapper.Map(articleDTO, article);
            await _repository.UpdateAsync(article);
        }

   

        public async Task<IEnumerable<ArticleDTO>> GetByMarqueLibelleAsync(string libelle) 
        {
            var spec = new ArticleByMarqueSpecification(libelle);
            var articles = await _repository.ListAsync(spec);
            return _mapper.Map<IEnumerable<ArticleDTO>>(articles);
        }

        public async Task<IEnumerable<ArticleDTO>> GetByTypeLibelleAsync(string libelle) 
        {
            var spec = new ArticleByTypeLibelleSpecification(libelle);
            var articles = await _repository.ListAsync(spec);
            return _mapper.Map<IEnumerable<ArticleDTO>>(articles);
        }

        public async Task<IEnumerable<ArticleDTO>> GetByFournisseurAdresseAsync(string adresse) 
        {
            var spec = new ArticleByFournisseurAdresseSpecification(adresse);
            var articles = await _repository.ListAsync(spec);
            return _mapper.Map<IEnumerable<ArticleDTO>>(articles);
        }

       


       

public async Task<IEnumerable<ArticleDetailsDTO>> SearchArticleDetailsByCriteriaAsync(string? societe, string? fournisseur, string? marque, string? reference)
{
    var query = _context.ArrivageDetails
        .Include(ad => ad.Article)
            .ThenInclude(a => a.Fournisseur)
        .Include(ad => ad.Article)
            .ThenInclude(a => a.Marque)
        .Include(ad => ad.Arrivage)
            .ThenInclude(a => a.Societe)
        .AsQueryable();

    if (!string.IsNullOrEmpty(societe))
    {
        query = query.Where(ad => ad.Arrivage.Societe.NomSociete.Trim() == societe.Trim());
    }

    if (!string.IsNullOrEmpty(fournisseur))
    {
        query = query.Where(ad => ad.Article.Fournisseur.NomFournisseur.Trim() == fournisseur.Trim());
    }

    if (!string.IsNullOrEmpty(marque))
    {
        query = query.Where(ad => ad.Article.Marque.Libelle.Trim() == marque.Trim());
    }

    if (!string.IsNullOrEmpty(reference))
    {
        query = query.Where(ad => ad.Article.Reference.Trim() == reference.Trim());
    }

    var articles = await query.Select(ad => new ArticleDetailsDTO
    {
        Id = ad.Id,
        ArrivageId = ad.ArrivageId,
        ArticleId = ad.ArticleId,
        Societe = ad.Arrivage.Societe.NomSociete,
        Fournisseur = ad.Article.Fournisseur.NomFournisseur,
        DateArrivage = ad.Arrivage.DateArrivage,
        Marque = ad.Article.Marque.Libelle,
        Reference = ad.Article.Reference,
        Description = ad.Article.Description,
        Type = ad.Article.Type.Libelle,
        Livre = ad.QuantiteRecuParArticle,
        Retourne = _context.RetourFournisseurs
                        .Where(rf => rf.ArrivageDetailsId == ad.Id)
                        .Sum(rf => rf.QuantiteRetournee),
            Distribue = _context.DistributionDetails
                        .Where(dd => dd.ArrivageDetailsId == ad.Id)
                        .Sum(dd => dd.Quantite),
            Vendu = _context.Ventes
                        .Where(v => v.ArticleId == ad.ArticleId)
                        .Sum(v => v.QuantiteVendu),
            Reste = ad.QuantiteRecuParArticle - (
                _context.DistributionDetails
                    .Where(dd => dd.ArrivageDetailsId == ad.Id)
                    .Sum(dd => dd.Quantite) +
                _context.RetourFournisseurs
                    .Where(rf => rf.ArrivageDetailsId == ad.Id)
                    .Sum(rf => rf.QuantiteRetournee) +
                _context.Ventes
                    .Where(v => v.ArticleId == ad.ArticleId)
                    .Sum(v => v.QuantiteVendu)),
        PrixAchatNetDevise = ad.PrixAchatNetDevise,
        PrixAchatNetMAD = ad.PrixAchatNetMAD
    }).ToListAsync();

    return articles ?? new List<ArticleDetailsDTO>();
}




public async Task<IEnumerable<ArticleDetailsDTO>> GetArticleDetailsAsync()
{
    var articles = await _context.ArrivageDetails
        .Include(ad => ad.Article)
        .Include(ad => ad.Arrivage)
            .ThenInclude(a => a.Societe)
        .Include(ad => ad.Article)
            .ThenInclude(a => a.Fournisseur)
        .Include(ad => ad.Article)
            .ThenInclude(a => a.Marque)
        .Select(ad => new ArticleDetailsDTO
        {
            Id = ad.Id,
            ArrivageId = ad.ArrivageId,
            ArticleId = ad.ArticleId,
            Societe = ad.Arrivage.Societe.NomSociete,
            Fournisseur = ad.Article.Fournisseur.NomFournisseur,
            DateArrivage = ad.Arrivage.DateArrivage,
            Marque = ad.Article.Marque.Libelle,
            Reference = ad.Article.Reference,
            Description = ad.Article.Description,
            Type = ad.Article.Type.Libelle,
            Livre = ad.QuantiteRecuParArticle,
            Retourne = _context.RetourFournisseurs
                        .Where(rf => rf.ArrivageDetailsId == ad.Id)
                        .Sum(rf => rf.QuantiteRetournee),
            Distribue = _context.DistributionDetails
                        .Where(dd => dd.ArrivageDetailsId == ad.Id)
                        .Sum(dd => dd.Quantite),
            Vendu = _context.Ventes
                        .Where(v => v.ArticleId == ad.ArticleId)
                        .Sum(v => v.QuantiteVendu),
            Reste = ad.QuantiteRecuParArticle - (
                _context.DistributionDetails
                    .Where(dd => dd.ArrivageDetailsId == ad.Id)
                    .Sum(dd => dd.Quantite) +
                _context.RetourFournisseurs
                    .Where(rf => rf.ArrivageDetailsId == ad.Id)
                    .Sum(rf => rf.QuantiteRetournee) +
                _context.Ventes
                    .Where(v => v.ArticleId == ad.ArticleId)
                    .Sum(v => v.QuantiteVendu)),
                    PrixAchatNetDevise = ad.PrixAchatNetDevise,
        PrixAchatNetMAD = ad.PrixAchatNetMAD
                    
        })
         // Ensure only articles with Reste > 0 are included
        .ToListAsync();

    return articles ?? new List<ArticleDetailsDTO>(); 
}






public async Task<IEnumerable<ArticleDTO>> SearchArticlesByCriteriaAsync(string? fournisseur, string? marque, string? type, string? reference)
{
    var query = _context.Articles
        .Include(a => a.Fournisseur)
        .Include(a => a.Marque)
        .Include(a => a.Type)
        .AsQueryable();

    if (!string.IsNullOrEmpty(fournisseur))
    {
        query = query.Where(a => a.Fournisseur.NomFournisseur.Trim() == fournisseur.Trim());
    }
    

    if (!string.IsNullOrEmpty(marque))
    {
        query = query.Where(a => a.Marque.Libelle.Trim() == marque.Trim());
    }

    if (!string.IsNullOrEmpty(type))
    {
        query = query.Where(a => a.Type.Libelle.Trim() == type.Trim());
    }

    if (!string.IsNullOrEmpty(reference))
    {
        query = query.Where(a => a.Reference.Trim() == reference.Trim());
    }

     var articles = await query.Select(a => new ArticleDTO
    {
        Id = a.Id,
        Description = a.Description,
        Reference = a.Reference,
        MarqueId = a.Marque.Id,  
        MarqueLibelle = a.Marque.Libelle,
        TypeId = a.Type.Id,  
        TypeLibelle = a.Type.Libelle,
        FournisseurId = a.Fournisseur.Id,  
        FournisseurNom = a.Fournisseur.NomFournisseur,
        PrixDeVenteInter = a.PrixDeVenteInter,
        PrixDeVenteRevendeur = a.PrixDeVenteRevendeur,
        PrixDeVenteClientFinal = a.PrixDeVenteClientFinal,
        Couleur = a.Couleur
    }).ToListAsync();
    Console.WriteLine("Nombre d'articles retournés: " + articles.Count());

    return articles ?? new List<ArticleDTO>();
}

public async Task<ArticleDetailsDTO?> GetArticleDetailsByArrivageDetailsIdAsync(int arrivageDetailsId)
{
    var articleDetail = await _context.ArrivageDetails
        .Include(ad => ad.Article)
        .Include(ad => ad.Arrivage)
            .ThenInclude(a => a.Societe)
        .Include(ad => ad.Article)
            .ThenInclude(a => a.Fournisseur)
        .Include(ad => ad.Article)
            .ThenInclude(a => a.Marque)
        .Where(ad => ad.Id == arrivageDetailsId)
        .Select(ad => new ArticleDetailsDTO
        {
            Id = ad.Id,
            ArrivageId = ad.ArrivageId,
            ArticleId = ad.ArticleId,
            Societe = ad.Arrivage.Societe.NomSociete,
            Fournisseur = ad.Article.Fournisseur.NomFournisseur,
            DateArrivage = ad.Arrivage.DateArrivage,
            Marque = ad.Article.Marque.Libelle,
            Reference = ad.Article.Reference,
            Description = ad.Article.Description,
            Type = ad.Article.Type.Libelle,
            Livre = ad.QuantiteRecuParArticle,
            Retourne = _context.RetourFournisseurs
                        .Where(rf => rf.ArrivageDetailsId == ad.Id)
                        .Sum(rf => rf.QuantiteRetournee),
            Distribue = _context.DistributionDetails
                        .Where(dd => dd.ArrivageDetailsId == ad.Id)
                        .Sum(dd => dd.Quantite),
            Vendu = _context.Ventes
                        .Where(v => v.ArticleId == ad.ArticleId)
                        .Sum(v => v.QuantiteVendu),
            Reste = ad.QuantiteRecuParArticle - (
                _context.DistributionDetails
                    .Where(dd => dd.ArrivageDetailsId == ad.Id)
                    .Sum(dd => dd.Quantite) +
                _context.RetourFournisseurs
                    .Where(rf => rf.ArrivageDetailsId == ad.Id)
                    .Sum(rf => rf.QuantiteRetournee) +
                _context.Ventes
                    .Where(v => v.ArticleId == ad.ArticleId)
                    .Sum(v => v.QuantiteVendu)),
            PrixAchatNetDevise = ad.PrixAchatNetDevise,
            PrixAchatNetMAD = ad.PrixAchatNetMAD
        })
        .FirstOrDefaultAsync();

    return articleDetail;
}



public async Task<IEnumerable<ArticleDetailsGroupedDTO>> GetArticlesGroupedByDescriptionTypeMarqueAsync(int? typeId, int? fournisseurId, int? marqueId, string reference)
{
    var query = _context.Articles
        .Include(a => a.Marque)
        .Include(a => a.Fournisseur)
        .Include(a => a.Type)
        .Include(a => a.ArrivageDetails)
            .ThenInclude(ad => ad.Arrivage)
            .ThenInclude(ar => ar.Societe)
        .AsQueryable();

    // Appliquer les filtres seulement s'ils sont fournis
    if (typeId.HasValue)
        query = query.Where(a => a.TypeId == typeId.Value);

    if (fournisseurId.HasValue)
        query = query.Where(a => a.FournisseurId == fournisseurId.Value);

    if (marqueId.HasValue)
        query = query.Where(a => a.MarqueId == marqueId.Value);

    if (!string.IsNullOrEmpty(reference))
        query = query.Where(a => a.Reference.Contains(reference));

    var articles = await query.ToListAsync();

    // Regroupement et calculs comme avant
    var groupedArticles = articles
        .AsEnumerable()
        .GroupBy(a => new 
        { 
            a.Description, 
            TypeId = a.Type.Id,  
            MarqueId = a.Marque.Id
        })
        .Select(g => new ArticleDetailsGroupedDTO
        {
            Description = g.Key.Description,
            TypeLibelle = g.First().Type.Libelle,
            MarqueLibelle = g.First().Marque.Libelle,
            FournisseurNom = g.First().Fournisseur.NomFournisseur,
            Couleur = g.First().Couleur,
            Reference = g.First().Reference,
            CodeBarre = g.First().ArrivageDetails.FirstOrDefault()?.NumSerie ?? string.Empty,
            PrixDeVenteRevendeur = g.First().PrixDeVenteRevendeur,
            PrixDeVenteClientFinal = g.First().PrixDeVenteClientFinal,
            Articles = g.Select(a => new ArticleDTO
            {
                Id = a.Id,
                Description = a.Description,
                Reference = a.Reference,
                MarqueId = a.Marque.Id,
                MarqueLibelle = a.Marque.Libelle,
                TypeId = a.Type.Id,
                TypeLibelle = a.Type.Libelle,
                FournisseurId = a.Fournisseur.Id,
                FournisseurNom = a.Fournisseur.NomFournisseur,
                PrixDeVenteInter = a.PrixDeVenteInter,
                PrixDeVenteRevendeur = a.PrixDeVenteRevendeur,
                PrixDeVenteClientFinal = a.PrixDeVenteClientFinal,
                Couleur = a.Couleur,
                QuantiteDistribuee = a.ArrivageDetails.Sum(ad => ad.QuantiteRecuParArticle)
            }).ToList(),
            TotalQuantiteLivree = g.SelectMany(a => a.ArrivageDetails).Sum(ad => ad.QuantiteRecuParArticle),
            TotalQuantiteRestante = g.SelectMany(a => a.ArrivageDetails).Sum(ad => ad.QuantiteRecuParArticle) - (
                _context.RetourFournisseurs
                    .AsEnumerable()
                    .Where(rf => g.SelectMany(a => a.ArrivageDetails).Select(ad => ad.Id).Contains(rf.ArrivageDetailsId))
                    .Sum(rf => rf.QuantiteRetournee) +
                _context.DistributionDetails
                    .AsEnumerable()
                    .Where(dd => g.SelectMany(a => a.ArrivageDetails).Select(ad => ad.Id).Contains(dd.ArrivageDetailsId))
                    .Sum(dd => dd.Quantite) +
                _context.Ventes
                    .AsEnumerable()
                    .Where(v => g.Select(a => a.Id).Contains(v.ArticleId))
                    .Sum(v => v.QuantiteVendu))
        }).ToList();

    return groupedArticles;
}


public async Task AddAsync(ArticleDTO articleDTO)
{
    // Retrieve related entities (e.g., Fournisseur, Marque, Type)
    var fournisseur = await _context.Fournisseurs.FindAsync(articleDTO.FournisseurId);
    var marque = await _context.RefMarques.FindAsync(articleDTO.MarqueId);
    var type = await _context.RefTypes.FindAsync(articleDTO.TypeId);

    // Validate if these entities exist
    if (fournisseur == null || marque == null || type == null)
    {
        throw new InvalidOperationException("Related entity not found.");
    }

    // Create a new Article object and set its properties
    var article = new Article
    {
        Description = articleDTO.Description,
        Reference = articleDTO.Reference,
        Marque = marque,
        Type = type,
        Couleur = articleDTO.Couleur,
        Fournisseur = fournisseur,
        PrixDeVenteInter = articleDTO.PrixDeVenteInter,
        PrixDeVenteRevendeur = articleDTO.PrixDeVenteRevendeur,
        PrixDeVenteClientFinal = articleDTO.PrixDeVenteClientFinal
    };

    // Add the new article to the context and save changes
    _context.Articles.Add(article);
    var articleEntry = _context.Entry(article);
_logger.LogInformation($"Entity State: {articleEntry.State}");

    await _context.SaveChangesAsync();
}


public async Task<IEnumerable<ArticleDTO>> GetByFournisseurIdAsync(int fournisseurId)
{
    // Récupère les articles avec une relation valide avec la Marque
    var articles = await _context.Articles
        .Include(a => a.Marque) // Inclure la relation avec la marque
        .Where(a => a.FournisseurId == fournisseurId) // Filtrer par fournisseur
        .Select(a => new ArticleDTO
        {
            Id = a.Id,
            Description = a.Description, // Charger la description de l'article
            MarqueLibelle = a.Marque != null ? a.Marque.Libelle : "Marque inconnue" // Gérer le cas où la marque est null
        })
        .ToListAsync();

    return articles;
}





    }}