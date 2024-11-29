using AutoMapper;
using optique.Dtos;
using optique.Models;

namespace optique.Mappers
{
    public class DistributionProfile : Profile
    {
        public DistributionProfile()
        {
            // Mapping for RefStatutDistribution
            CreateMap<Distribution, DistributionDTO>().ReverseMap();
        }
    }
}
