using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class ArrivageDetailsProfile : Profile
    {
        public ArrivageDetailsProfile()
        {
            CreateMap<ArrivageDetails, ArrivageDetailsDTO>().ReverseMap();
        }
    }
}
