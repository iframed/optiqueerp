using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class RefDeviseProfile : Profile
    {
        public RefDeviseProfile()
        {
            CreateMap<RefDevise, RefDeviseDTO>().ReverseMap();
        }
    }
}
