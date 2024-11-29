using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class DistributionDetailsProfile : Profile
    {
        public DistributionDetailsProfile()
        {
            CreateMap<DistributionDetails, DistributionDetailsDTO>().ReverseMap();
        }
    }
}
