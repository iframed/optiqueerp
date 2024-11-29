using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class FournisseurProfile : Profile
    {
        public FournisseurProfile()
        {
           /* CreateMap<Fournisseur, FournisseurDTO>()
            .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.NumTelephone))
                .ForMember(dest => dest.DeviseLibelle, opt => opt.MapFrom(src => src.Devise.Libelle))
                .ForMember(dest => dest.DeviseCode, opt => opt.MapFrom(src => src.Devise.Code))


                
                .ReverseMap();*/

                CreateMap<Fournisseur, FournisseurDTO>()
            .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.NumTelephone))
            .ForMember(dest => dest.DeviseLibelle, opt => opt.MapFrom(src => src.Devise.Libelle))
            .ForMember(dest => dest.DeviseCode, opt => opt.MapFrom(src => src.Devise.Code))
            .ReverseMap();
        }
    }
}
