using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class VenteProfile : Profile
    {
        public VenteProfile()
        {
            CreateMap<Vente, VenteDTO>().ReverseMap();
            CreateMap<DetailsPaiement, DetailsPaiementDTO>().ReverseMap();
           
        

            // Ajout de mappages spécifiques pour Vente et VenteDetailsDTO
            CreateMap<Vente, VenteDetailsDTO>()
                .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Client.NomClient))
                .ForMember(dest => dest.Marque, opt => opt.MapFrom(src => src.Article.Marque.Libelle))
                .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.Article.Reference))
                .ForMember(dest => dest.TypeArticle, opt => opt.MapFrom(src => src.Article.Type.Libelle))
                .ForMember(dest => dest.TypeDePaiement, opt => opt.MapFrom(src => src.DetailsPaiements.FirstOrDefault().TypeDePaiement.Libelle))
                .ForMember(dest => dest.DetailsPaiement, opt => opt.MapFrom(src => src.DetailsPaiements.FirstOrDefault().NCheque));




           CreateMap<MouvementArticle, MouvementArticleDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Vente.Id))
                .ForMember(dest => dest.VenteCreePar, opt => opt.MapFrom(src => src.Vente.CreePar));
        }     

        }
    }
