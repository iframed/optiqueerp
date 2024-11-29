using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientDTO>()
                .ForMember(dest => dest.Telephone, opt => opt.MapFrom(src => src.NumTelephone))
                .ReverseMap()
                .ForMember(dest => dest.NumTelephone, opt => opt.MapFrom(src => src.Telephone));


            
        }
    }
}
