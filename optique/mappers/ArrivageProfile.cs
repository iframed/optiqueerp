using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class ArrivageProfile : Profile
    {
        public ArrivageProfile()
        {
            CreateMap<Arrivage, ArrivageDTO>().ReverseMap();
        }
    }
}
