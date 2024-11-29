using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class RefStatutDistributionProfile : Profile
    {
        public RefStatutDistributionProfile()
        {
            CreateMap<RefStatutDistribution, RefStatutDistributionDTO>().ReverseMap();
        }
    }
}
