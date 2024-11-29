using AutoMapper;
using MyAspNetApp.Repositories;
using optique.Dtos;
using optique.Models;
using optique.IServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using optique.Specifications;
using optique.Data;
using Microsoft.EntityFrameworkCore;

namespace optique.Services
{
    public class FournisseurService : IFournisseurService
    {
        private readonly IRepository<Fournisseur> _repository;
        private readonly IMapper _mapper;

         private readonly ApplicationDbContext _context;

        public FournisseurService(IRepository<Fournisseur> repository, IMapper mapper , ApplicationDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

      

        public async Task<IEnumerable<FournisseurDTO>> GetAllAsync()
{
    var fournisseurs = await _repository.ListAsync(include: query => query.Include(f => f.Devise));
    return _mapper.Map<IEnumerable<FournisseurDTO>>(fournisseurs);
}

        public async Task<FournisseurDTO?> GetByIdAsync(int id)
        {
            var fournisseur = await _repository.GetByIdAsync(id);
            return _mapper.Map<FournisseurDTO?>(fournisseur);
        }

       

       

    
   


        public async Task UpdateAsync(FournisseurDTO fournisseurDTO)
        {
            var fournisseur = await _repository.GetByIdAsync(fournisseurDTO.Id);
            if (fournisseur == null)
            {
                throw new Exception("Le fournisseur spécifié n'existe pas.");
            }

            _mapper.Map(fournisseurDTO, fournisseur);
            await _repository.UpdateAsync(fournisseur);
        }

        public async Task DeleteAsync(int id)
        {
            var fournisseur = await _repository.GetByIdAsync(id);
            if (fournisseur == null)
            {
                throw new Exception("Le fournisseur spécifié n'existe pas.");
            }

            await _repository.DeleteAsync(fournisseur);
        }

        public async Task<IEnumerable<FournisseurDTO>> GetByDeviseLibelleAsync(string deviseLibelle)
{
    var spec = new FournisseurByDeviseSpecification(deviseLibelle);
    var fournisseurs = await _repository.ListAsync(spec);
    return _mapper.Map<IEnumerable<FournisseurDTO>>(fournisseurs);
}

        public async Task<IEnumerable<FournisseurDTO>> GetByICEAsync(string ice)
        {
           /* var fournisseur = await _repository.FirstOrDefaultAsync(new FournisseurByICESpecification(ice));
            return _mapper.Map<FournisseurDTO?>(fournisseur);*/

            // Fetch all matching fournisseurs
    var fournisseurs = await _repository.ListAsync(new FournisseurByICESpecification(ice));
    return _mapper.Map<IEnumerable<FournisseurDTO>>(fournisseurs);
        }



         public async Task<IEnumerable<FournisseurDetailsDTO>> GetFournisseurDetailsAsync()
    {
        var arrivages = await _context.Arrivages
            .Include(a => a.Societe)
            .Include(a => a.Fournisseur)
            .Include(a => a.ArrivageDetails)
                .ThenInclude(ad => ad.Article)
                    .ThenInclude(ar => ar.Marque)
            .Include(a => a.ArrivageDetails)
               // .ThenInclude(ad => ad.RetourFournisseurs)
                .ThenInclude(ad => (ad.RetourFournisseurs ?? new List<RetourFournisseur>()))
                    .ThenInclude(rf => rf.TypeRetour)
            .ToListAsync();

        return _mapper.Map<IEnumerable<FournisseurDetailsDTO>>(arrivages);
    }


    public async Task<IEnumerable<FournisseurDTO>> GetByICEAndDeviseLibelleAsync(string ice, string deviseLibelle)
{
    var fournisseurs = await _context.Fournisseurs
        .Include(f => f.Devise)
        .Where(f => f.ICE == ice && f.Devise.Libelle == deviseLibelle)
        .ToListAsync();

    return _mapper.Map<IEnumerable<FournisseurDTO>>(fournisseurs);
}




public async Task AddAsync(FournisseurDTO fournisseurDTO)
{
    // Rechercher la devise par son Libelle et son Code
    var devise = await _context.RefDevises
        .FirstOrDefaultAsync(d => d.Libelle == fournisseurDTO.DeviseLibelle && d.Code == fournisseurDTO.DeviseCode);

    // Si la devise n'existe pas, la créer
    if (devise == null)
    {
        devise = new RefDevise
        {
            Libelle = fournisseurDTO.DeviseLibelle,
            Code = fournisseurDTO.DeviseCode
        };

        _context.RefDevises.Add(devise);
        await _context.SaveChangesAsync(); // Sauvegarder pour obtenir l'Id de la nouvelle devise
    }

    // Mettre à jour le DTO avec les informations de la devise
    fournisseurDTO.DeviseLibelle = devise.Libelle;
    fournisseurDTO.DeviseCode = devise.Code;

    // Créer le fournisseur avec la devise trouvée ou nouvellement créée
    var fournisseur = new Fournisseur
    {
        NomFournisseur = fournisseurDTO.NomFournisseur,
        Adresse = fournisseurDTO.Adresse,
        NumTelephone = fournisseurDTO.Telephone,
        ICE = fournisseurDTO.ICE,
        Pays = fournisseurDTO.Pays,
        DeviseId = devise.Id, // Associer l'Id de la devise
        Devise = devise,
        Articles = new List<Article>(), // Initialiser les collections pour éviter les nulls
        Arrivages = new List<Arrivage>()
    };

    // Ajouter le fournisseur à la base de données
    _context.Fournisseurs.Add(fournisseur);
    await _context.SaveChangesAsync();

    // Mettre à jour le DTO avec l'ID du fournisseur créé
    fournisseurDTO.Id = fournisseur.Id;
}

       public async Task<string?> GetNomByIdAsync(int id)
{
    var fournisseur = await _repository.GetByIdAsync(id);
    return fournisseur?.NomFournisseur; // Retourne le nom du fournisseur ou null si pas trouvé
}

    }




    }

