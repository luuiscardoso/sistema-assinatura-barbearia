using APIAssinaturaBarbearia.Domain.DTO;
using APIAssinaturaBarbearia.Domain.Entities;
using AutoMapper;

namespace APIAssinaturaBarbearia.Application.Mappings
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
