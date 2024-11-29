using AutoMapper;
using optique.Models;
using optique.Dtos;

namespace optique.Mappers
{
    public class RefTypeProfile : Profile
    {public RefTypeProfile()
        {
            CreateMap<RefType, RefTypeDTO>().ReverseMap();
        }
    }
}
