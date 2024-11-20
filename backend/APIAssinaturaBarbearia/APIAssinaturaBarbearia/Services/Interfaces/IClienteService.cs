using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace APIAssinaturaBarbearia.Services.Interfaces
{
    public interface IClienteService
    {
        Cliente RegistrarCliente(ClienteDTO clienteDto, Assinatura assinaturaCriada);
    }
}
