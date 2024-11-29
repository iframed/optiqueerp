using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
           // CreateMap<Article, ArticleDTO>().ReverseMap();
            CreateMap<Article, ArticleDTO>()
            .ForMember(dest => dest.MarqueLibelle, opt => opt.MapFrom(src => src.Marque.Libelle))
            .ForMember(dest => dest.TypeLibelle, opt => opt.MapFrom(src => src.Type.Libelle))
            .ForMember(dest => dest.FournisseurNom, opt => opt.MapFrom(src => src.Fournisseur.NomFournisseur));
        CreateMap<ArticleDTO, Article>();


           
        }
        }
    }

