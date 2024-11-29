using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class RetourFournisseurProfile : Profile
    {
        public RetourFournisseurProfile()
        {
              CreateMap<Arrivage, FournisseurDetailsDTO>()
            .ForMember(dest => dest.Societe, opt => opt.MapFrom(src => src.Societe.NomSociete))
            .ForMember(dest => dest.Fournisseur, opt => opt.MapFrom(src => src.Fournisseur.NomFournisseur))
            .ForMember(dest => dest.NumFacture, opt => opt.MapFrom(src => src.NumFacture))
            .ForMember(dest => dest.NumBL, opt => opt.MapFrom(src => src.NumBL))
            .ForMember(dest => dest.DateArrivage, opt => opt.MapFrom(src => src.DateArrivage))
            .ForMember(dest => dest.DateRetour, opt => opt.MapFrom(src => src.ArrivageDetails.First().RetourFournisseurs.First().DateRetour))
            .ForMember(dest => dest.Reference, opt => opt.MapFrom(src => src.ArrivageDetails.First().Article.Reference))
            .ForMember(dest => dest.QuantiteRetournee, opt => opt.MapFrom(src => src.ArrivageDetails.First().RetourFournisseurs.First().QuantiteRetournee))
            .ForMember(dest => dest.Marque, opt => opt.MapFrom(src => src.ArrivageDetails.First().Article.Marque.Libelle))
            .ForMember(dest => dest.PrixDachatDeviseTTC, opt => opt.MapFrom(src => src.ArrivageDetails.First().PrixDachatDevise))
            .ForMember(dest => dest.PrixDachatMADTTC, opt => opt.MapFrom(src => src.ArrivageDetails.First().PrixDachatMAD))
            .ForMember(dest => dest.MotifRetour, opt => opt.MapFrom(src => src.ArrivageDetails.First().RetourFournisseurs.First().MotifRetour))
            .ForMember(dest => dest.TypeRetour, opt => opt.MapFrom(src => src.ArrivageDetails.First().RetourFournisseurs.First().TypeRetour.Libelle));





           CreateMap<RetourFournisseurDTO, RetourFournisseur>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.QuantiteRetournee, opt => opt.MapFrom(src => src.QuantiteRetourne))
            .ForMember(dest => dest.MotifRetour, opt => opt.MapFrom(src => src.MotifRetour)) // Mappage de MotifRetour
            .ForMember(dest => dest.DateRetour, opt => opt.Ignore()) // Assume DateRetour is not mapped directly from DTO
            .ForMember(dest => dest.TypeRetourId, opt => opt.MapFrom(src => src.TypeRetourId))
            .ForMember(dest => dest.ArrivageDetailsId, opt => opt.MapFrom(src => src.ArrivageDetailsId));

        CreateMap<RetourFournisseur, RetourFournisseurDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.QuantiteRetourne, opt => opt.MapFrom(src => src.QuantiteRetournee))
            .ForMember(dest => dest.TypeRetourId, opt => opt.MapFrom(src => src.TypeRetourId))
            .ForMember(dest => dest.ArrivageDetailsId, opt => opt.MapFrom(src => src.ArrivageDetailsId))
            .ForMember(dest => dest.MotifRetour, opt => opt.MapFrom(src => src.MotifRetour)); // Mappage de MotifRetour
    }


    }
}
