using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class DistributionSummaryProfile : Profile
    {
        public DistributionSummaryProfile()
        {
CreateMap<DistributionDetails, DistributionSummaryDTO>()
            .ForMember(dest => dest.Fournisseur, opt => opt.MapFrom(src => src.ArrivageDetails.Article.Fournisseur.NomFournisseur))
            .ForMember(dest => dest.Client, opt => opt.MapFrom(src => src.Distribution.Client.NomClient))
            .ForMember(dest => dest.Marque, opt => opt.MapFrom(src => src.ArrivageDetails.Article.Marque.Libelle))
            .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.ArrivageDetails.Article.Reference))
            .ForMember(dest => dest.DateDistribution, opt => opt.MapFrom(src => src.Distribution.DateDistribution))
            .ForMember(dest => dest.Quantite, opt => opt.MapFrom(src => src.Quantite))
            .ForMember(dest => dest.PrixDeVente, opt => opt.MapFrom(src => src.PrixDeVente))
            .ForMember(dest => dest.NumFacture, opt => opt.MapFrom(src => src.NumFacture))
            .ForMember(dest => dest.Statut, opt => opt.MapFrom(src => src.Distribution.StatutDistribution.Libelle)); // Utiliser le Libelle de RefStatutDistribution
    }

    }
}
