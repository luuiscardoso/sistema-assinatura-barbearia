using APIAssinaturaBarbearia.Models;
using AutoMapper;

namespace APIAssinaturaBarbearia.DTO.Mappings
{
    public class AssinaturaMappingProfile : Profile
    {
        public AssinaturaMappingProfile()
        {
            CreateMap<Assinatura, AssinaturaUpdateDTO>().ReverseMap();  
        }
    }
}
