using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class RefTypeDePaiementProfile : Profile
    {
        public RefTypeDePaiementProfile()
        {
            CreateMap<RefTypeDePaiement, RefTypeDePaiementDTO>().ReverseMap();
        }
    }
}
