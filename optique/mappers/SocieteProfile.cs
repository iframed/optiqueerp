using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class SocieteProfile : Profile
    {
        public SocieteProfile()
        {
            CreateMap<Societe, SocieteDTO>().ReverseMap();
        }
    }
}
