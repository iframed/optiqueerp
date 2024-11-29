using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class RefTypeClientProfile : Profile
    {
        public RefTypeClientProfile()
        {
            CreateMap<RefTypeClient, RefTypeClientDTO>().ReverseMap();
        }
    }
}
