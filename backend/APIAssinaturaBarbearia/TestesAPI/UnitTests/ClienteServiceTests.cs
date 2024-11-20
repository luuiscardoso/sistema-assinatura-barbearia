using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Models;
using APIAssinaturaBarbearia.Repositories.Interfaces;
using APIAssinaturaBarbearia.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestesAPI.UnitTests
{
    public class ClienteServiceTests
    {
        [Fact]
        public void RegistrarCliente_InformandoDadosClienteEAssinaturaCriada_RetornaNovoCliente()
        {
            //Arrange
            var mockUnityOfWork = new Mock<IUnityOfWork>();
           
            ClienteDTO clienteDto = new ClienteDTO()
            {
                Cpf = "12345678912",
                Nome = "teste"
            };

            Assinatura assinaturaCriada = new Assinatura() { AssinaturaId = 1 }; 

            Cliente clienteCriado = new Cliente()
            {
                AssinaturaId = assinaturaCriada.AssinaturaId,
                Cpf = clienteDto.Cpf,
                Nome = clienteDto.Nome
            };

            mockUnityOfWork.Setup(u => u.ClienteRepository.Criar(It.IsAny<Cliente>())).Returns(clienteCriado);

            var clienteService = new ClienteService(mockUnityOfWork.Object);

            //Act
            Cliente result = clienteService.RegistrarCliente(clienteDto, assinaturaCriada);

            //Assert
            mockUnityOfWork.Verify(u => u.ClienteRepository.Criar(It.Is<Cliente>(c => c.AssinaturaId == assinaturaCriada.AssinaturaId)), Times.Once);
            Assert.Equal(result.Cpf, clienteDto.Cpf);
            Assert.Equal(result.Nome, clienteDto.Nome);
        }
    }
}
