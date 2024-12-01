using APIAssinaturaBarbearia.DTO;
using APIAssinaturaBarbearia.Exceptions;
using APIAssinaturaBarbearia.Models;
using APIAssinaturaBarbearia.Repositories.Interfaces;
using APIAssinaturaBarbearia.Services;
using APIAssinaturaBarbearia.Services.Interfaces;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestesAPI.UnitTests
{
    public class AssinaturaClienteHandlerServiceTests
    {
        [Fact]
        public void RegistraCliente_InformandoDadosClienteSemAssinatura_CriaAssinaturaParaCliente()
        {
            //Arrange
            var mockUnityOfWork = new Mock<IUnityOfWork>();
            var mockClienteService = new Mock<IClienteService>();
            ClienteCadastroDTO clienteDto = new ClienteCadastroDTO()
            {
                Cpf = "32365678900",
                Nome = "teste"
            };

            IEnumerable<Assinatura> assinaturas = new List<Assinatura>()
            {
                new Assinatura
                {
                    AssinaturaId = 1, Cliente = new Cliente{ AssinaturaId = 1, Cpf = "12345678912"}
                },
                new Assinatura
                {
                    AssinaturaId = 2, Cliente = new Cliente{ AssinaturaId = 2, Cpf = "75175688312"}
                }
            };

            mockUnityOfWork.Setup(u => u.AssinaturaRepository.Todos(It.IsAny<string>()))
                            .ReturnsAsync(assinaturas);

            mockUnityOfWork.Setup(u => u.AssinaturaRepository.Criar(It.IsAny<Assinatura>())).Verifiable();
            mockClienteService.Setup(c => c.RegistrarCliente(It.IsAny<ClienteCadastroDTO>(), It.IsAny<Assinatura>()))
                               .Returns(new Cliente());

            var assinaturaClienteHandler = new AssinaturaClienteHandlerService(mockUnityOfWork.Object, mockClienteService.Object);

            //Act
            var result = assinaturaClienteHandler.RegistrarNovaAssinatura(clienteDto);

            //Assert
            mockUnityOfWork.Verify(u => u.AssinaturaRepository.Criar(It.IsAny<Assinatura>()), Times.Once());
            mockClienteService.Verify(c =>
                c.RegistrarCliente(It.Is<ClienteCadastroDTO>(dto => dto.Cpf == clienteDto.Cpf),
                                    It.IsAny<Assinatura>()), Times.Once()
            );
            mockUnityOfWork.Verify(u => u.Commit(), Times.Once());
        }

        [Fact]
        public async void RegistraCliente_InformandoDadosClienteComAssinatura_RetornaAlreadyHasSubscriptionException()
        {
            //Arrange
            var mockUnityOfWork = new Mock<IUnityOfWork>();
            var mockClienteService = new Mock<IClienteService>();
            ClienteCadastroDTO clienteDto = new ClienteCadastroDTO()
            {
                Cpf = "12345678912",
                Nome = "teste"
            };

            IEnumerable<Assinatura> assinaturas = new List<Assinatura>()
            {
                new Assinatura
                {
                    AssinaturaId = 1, Cliente = new Cliente{ AssinaturaId = 1, Cpf = "12345678912"}
                },
                new Assinatura
                {
                    AssinaturaId = 2, Cliente = new Cliente{ AssinaturaId = 2, Cpf = "75175688312"}
                }
            };

            mockUnityOfWork.Setup(u => u.AssinaturaRepository.Todos(It.IsAny<string>()))
                            .ReturnsAsync(assinaturas);

            var assinaturaClienteHandler = new AssinaturaClienteHandlerService(mockUnityOfWork.Object, mockClienteService.Object);

            //Act & Assert
            await Assert.ThrowsAsync<AlreadyHasSubscriptionException>(async () => await assinaturaClienteHandler.RegistrarNovaAssinatura(clienteDto));
        }

        [Fact]
        public void ProcessaAtualizacaoAssinatura_ComNovoNomeCpf_DeveConcluirAtualizacao()
        {
            //Arrange
            var mockUnityOfWork = new Mock<IUnityOfWork>();
            var clienteService = new Mock<IClienteService>();   
        
            Assinatura assinaturaAlterar = new Assinatura
            {
                AssinaturaId = 1,
                Inicio = new DateTime(2024, 11, 19),
                Fim = new DateTime(2024, 12, 19),
                Status = true,
                Cliente = new Cliente
                {
                    Cpf = "12345678900",
                    Nome = "teste"
                }
            };

            AssinaturaUpdateDTO assinaturaDto = new AssinaturaUpdateDTO
            {
                Cpf = "12345678911",
                Nome = "alterado",
            };

            mockUnityOfWork.Setup(u => u.AssinaturaRepository.Atualizar(It.IsAny<Assinatura>())).Verifiable();
            var assinaturaClienteHandler = new AssinaturaClienteHandlerService(mockUnityOfWork.Object, clienteService.Object);

            //Act
            assinaturaClienteHandler.ProcessarAtualizacaoAssinatura(assinaturaAlterar, assinaturaDto);

            //Assert
            mockUnityOfWork.Verify(u => u.AssinaturaRepository.Atualizar(It.Is<Assinatura>(
                    a => a.Cliente.Nome == assinaturaDto.Nome &&
                         a.Cliente.Cpf == assinaturaDto.Cpf 
                )), Times.Once());
        }

        [Fact]
        public void ProcessaAtualizacaoAssinatura_ComNovoNome_DeveConcluirAtualizacao()
        {
            //Arrange
            var mockUnityOfWork = new Mock<IUnityOfWork>();
            var clienteService = new Mock<IClienteService>();

            Assinatura assinaturaAlterar = new Assinatura
            {
                AssinaturaId = 1,
                Inicio = new DateTime(2024, 11, 19),
                Fim = new DateTime(2024, 12, 19),
                Status = true,
                Cliente = new Cliente
                {
                    Cpf = "12345678900",
                    Nome = "teste"
                }
            };

            AssinaturaUpdateDTO assinaturaDto = new AssinaturaUpdateDTO
            {
                Nome = "alterado",
            };

            mockUnityOfWork.Setup(u => u.AssinaturaRepository.Atualizar(It.IsAny<Assinatura>())).Verifiable();
            var assinaturaClienteHandler = new AssinaturaClienteHandlerService(mockUnityOfWork.Object, clienteService.Object);

            //Act
            assinaturaClienteHandler.ProcessarAtualizacaoAssinatura(assinaturaAlterar, assinaturaDto);

            //Assert
            mockUnityOfWork.Verify(u => u.AssinaturaRepository.Atualizar(It.Is<Assinatura>(
                    a => a.Cliente.Nome == assinaturaDto.Nome && 
                         a.Cliente.Cpf != assinaturaDto.Cpf 
                )), Times.Once());
        }

        [Fact]
        public void ProcessaAtualizacaoAssinatura_ComNovoCpf_DeveConcluirAtualizacao()
        {
            //Arrange
            var mockUnityOfWork = new Mock<IUnityOfWork>();
            var clienteService = new Mock<IClienteService>();

            Assinatura assinaturaAlterar = new Assinatura
            {
                AssinaturaId = 1,
                Inicio = new DateTime(2024, 11, 19),
                Fim = new DateTime(2024, 12, 19),
                Status = true,
                Cliente = new Cliente
                {
                    Cpf = "12345678900",
                    Nome = "teste"
                }
            };

            AssinaturaUpdateDTO assinaturaDto = new AssinaturaUpdateDTO
            {
                Cpf = "00145678999",
            };

            mockUnityOfWork.Setup(u => u.AssinaturaRepository.Atualizar(It.IsAny<Assinatura>())).Verifiable();
            var assinaturaClienteHandler = new AssinaturaClienteHandlerService(mockUnityOfWork.Object, clienteService.Object);

            //Act
            assinaturaClienteHandler.ProcessarAtualizacaoAssinatura(assinaturaAlterar, assinaturaDto);

            //Assert
            mockUnityOfWork.Verify(u => u.AssinaturaRepository.Atualizar(It.Is<Assinatura>(
                    a => a.Cliente.Nome != assinaturaDto.Nome &&
                         a.Cliente.Cpf == assinaturaDto.Cpf
                )), Times.Once());
        }
    }
}
