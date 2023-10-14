using Animals.Dtos;
using Animals.Entities;
using Animals.Models;

namespace Animals.BLL.Impl.Mappers;

public class MappersProfile : AutoMapper.Profile
{
    public MappersProfile()
    {
        CreateMap<Dog, DogModel>().ReverseMap();
        CreateMap<DogModel, CreateDogDto>().ReverseMap();
    }
}
