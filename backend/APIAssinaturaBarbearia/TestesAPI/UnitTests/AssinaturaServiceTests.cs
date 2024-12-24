using APIAssinaturaBarbearia.Application.Exceptions;
using APIAssinaturaBarbearia.Application.Services;
using APIAssinaturaBarbearia.Domain.Entities;
using APIAssinaturaBarbearia.Domain.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TestesAPI.UnitTests
{
    public class AssinaturaServiceTests
    {
        [Fact]
        public async Task BuscarAssinaturaEspecifica_InformandoIdAssinaturaExistente_RetornaAssinatura()
        {
            //Arrange
            var id = 1;
            var mockUnityOfWork = new Mock<IUnityOfWork>();
            var assinaturaEsperada = new Assinatura(){AssinaturaId = id};

            mockUnityOfWork
                .Setup(u => u.AssinaturaRepository.Obter(a => a.AssinaturaId == id, "Cliente"))
                .ReturnsAsync(assinaturaEsperada);

            var assinaturaService = new AssinaturaService(mockUnityOfWork.Object);

            //Act
            Assinatura? result = await assinaturaService.BuscarAssinaturaEspecifica(id);

            //Assert
            Assert.Equal(1, result.AssinaturaId);
            mockUnityOfWork.Verify(u => u.AssinaturaRepository.Obter(a => a.AssinaturaId == id, "Cliente"), Times.Once());
        }

        [Fact]
        public async Task BuscarAssinaturaEspecifica_InformandoIdAssinaturaInexistente_RetornaNotFoundException()
        {
            //Arrange
            var id = 99;
            var mockUnityOfWork = new Mock<IUnityOfWork>();
            Assinatura assinaturaEsperada = null;

            mockUnityOfWork
                .Setup(u => u.AssinaturaRepository.Obter(a => a.AssinaturaId == id, "Cliente"))
                .ReturnsAsync(assinaturaEsperada);

            var assinaturaService = new AssinaturaService(mockUnityOfWork.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ApplicationNotFoundException>(async () => await assinaturaService.BuscarAssinaturaEspecifica(id));
            mockUnityOfWork.Verify(u => u.AssinaturaRepository.Obter(a => a.AssinaturaId == id, "Cliente"), Times.Once());
        }

        [Fact]
        public async Task BuscarAssinaturas_RetornaColecaoAssinaturas()
        {
            //Arrange
            int numeroPagina = 1;
            var mockUnityOfWork = new Mock<IUnityOfWork>();
            IEnumerable<Assinatura> assinaturas = new List<Assinatura>()
            {
                new Assinatura(){AssinaturaId = 1},
                new Assinatura(){AssinaturaId = 2}
            };

            mockUnityOfWork
                .Setup(u => u.AssinaturaRepository.Todos("Cliente"))
                .ReturnsAsync(assinaturas);

            var assinaturaService = new AssinaturaService(mockUnityOfWork.Object);

            //Act
            var result = await assinaturaService.BuscarAssinaturas(numeroPagina);

            //Assert
            Assert.NotEmpty(result.Registros);
            mockUnityOfWork.Verify(u => u.AssinaturaRepository.Todos("Cliente"), Times.Once());
        }

        [Fact]
        public async Task BuscarAssinaturas_RetornaNotFoundException()
        {
            //Arrange
            int numeroPagina = 1;
            var mockUnityOfWork = new Mock<IUnityOfWork>();
            IEnumerable<Assinatura> assinaturas = Enumerable.Empty<Assinatura>();

            mockUnityOfWork
                .Setup(u => u.AssinaturaRepository.Todos("Cliente"))
                .ReturnsAsync(assinaturas);

            var assinaturaService = new AssinaturaService(mockUnityOfWork.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ApplicationNotFoundException>(async () => await assinaturaService.BuscarAssinaturas(numeroPagina));
            mockUnityOfWork.Verify(u => u.AssinaturaRepository.Todos("Cliente"), Times.Once());
        }

        [Fact]
        public async Task ExcluiAssinatura_InformandoIdAssinaturaExistente_DeveExcluirAssinatura()
        {
            //Arrange
            var mockUnityOfWork = new Mock<IUnityOfWork>();
            Assinatura assinaturaExcluir = new Assinatura() { AssinaturaId = 1};

            mockUnityOfWork
                .Setup(u => u.AssinaturaRepository.Obter(It.IsAny<Expression<Func<Assinatura, bool>>>(), It.IsAny<string>()))
                .ReturnsAsync(assinaturaExcluir);

            var assinaturaService = new AssinaturaService(mockUnityOfWork.Object);

            //Act
            await assinaturaService.ExcluirAssinatura(1);


            //Assert
            mockUnityOfWork.Verify(u => u.AssinaturaRepository.
                            Obter(It.IsAny<Expression<Func<Assinatura, bool>>>(), It.IsAny<string>()), Times.Once());
            mockUnityOfWork.Verify(u => u.AssinaturaRepository.Excluir(It.Is<Assinatura>(a => a.AssinaturaId == 1)), 
                                                                                                      Times.Once());
            mockUnityOfWork.Verify(u => u.Commit(), Times.Once());
        }

        [Fact]
        public async Task ExcluiAssinatura_InforfmandoAssinaturaInexistente_RetornaNotFoundException()
        {
            //Arrange
            var id = 99;
            var mockUnityOfWork = new Mock<IUnityOfWork>();
            Assinatura assinaturaExcluir = null;

            mockUnityOfWork.Setup(u => u.AssinaturaRepository.Obter(It.IsAny <Expression<Func<Assinatura, bool>>>(), It.IsAny<string>()))
                           .ReturnsAsync(assinaturaExcluir);

            var assinaturaService = new AssinaturaService(mockUnityOfWork.Object);

            //Act & Assert
            await Assert.ThrowsAsync<ApplicationNotFoundException>(async () => await assinaturaService.ExcluirAssinatura(id));
        }
    }
}
