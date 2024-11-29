using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class MouvementArticleProfile : Profile
    {
        public MouvementArticleProfile()
        {
           CreateMap<MouvementArticle, MouvementArticleDTO>()
            .ForMember(dest => dest.VenteCreePar, opt => opt.MapFrom(src => src.Vente!.CreePar));
        }
    }
}
