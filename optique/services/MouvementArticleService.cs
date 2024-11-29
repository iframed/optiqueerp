using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using optique.Data;
using optique.Dtos;
using optique.IServices;
using optique.Models;

namespace optique.Services
{
    public class MouvementArticleService : IMouvementArticleService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public MouvementArticleService(ApplicationDbContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<IEnumerable<MouvementArticleDTO>> GetAllAsync()
        {
            var mouvements = await _context.MouvementArticles
                .Include(ma => ma.Vente) 
                .ToListAsync();

            return _mapper.Map<IEnumerable<MouvementArticleDTO>>(mouvements);
        }

        public async Task AddAsync(MouvementArticleDTO mouvementArticleDTO)
        {
            var mouvementArticle = _mapper.Map<MouvementArticle>(mouvementArticleDTO);
            mouvementArticle.CreePar = _userService.GetUserName(); // Récupérer le nom de l'utilisateur connecté
            mouvementArticle.DateDeCreation = DateTime.Now;

            await _context.MouvementArticles.AddAsync(mouvementArticle);
            await _context.SaveChangesAsync();
        }

        public async Task<MouvementArticleDTO?> GetByIdAsync(int id)
        {
            var mouvementArticle = await _context.MouvementArticles
                .Include(ma => ma.Vente)
                .FirstOrDefaultAsync(ma => ma.Id == id);

            return _mapper.Map<MouvementArticleDTO?>(mouvementArticle);
        }
    }
}
