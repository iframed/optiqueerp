using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class RefMarqueProfile : Profile
    {
        public RefMarqueProfile()
        {
            CreateMap<RefMarque, RefMarqueDTO>().ReverseMap();

           

        }
    }
}
