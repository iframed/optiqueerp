using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class RefTypeRetourProfile : Profile
    {
        public RefTypeRetourProfile()
        {
            CreateMap<RefTypeRetour, RefTypeRetourDTO>().ReverseMap();
        }
    }
}
