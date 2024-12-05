using APIAssinaturaBarbearia.Domain.Entities;
using AutoMapper;

namespace APIAssinaturaBarbearia.Application.DTO.Mappings
{
    public class AssinaturaMappingProfile : Profile
    {
        public AssinaturaMappingProfile()
        {
            CreateMap<Assinatura, AssinaturaUpdateDTO>().ReverseMap(); 
            CreateMap<Cliente, ClienteCadastroDTO>().ReverseMap();
        }
    }
}
