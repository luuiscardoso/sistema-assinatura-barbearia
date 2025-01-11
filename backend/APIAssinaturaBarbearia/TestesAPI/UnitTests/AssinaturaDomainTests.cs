using APIAssinaturaBarbearia.Application.Exceptions;
using APIAssinaturaBarbearia.Domain.Entities;
using APIAssinaturaBarbearia.Domain.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestesAPI.UnitTests
{
    public class AssinaturaDomainTests
    {
        [Fact]
        public void InstanciaAssinatura_InformandoExpiracaoMenorInicio_RetornaDomainPeriodOfInvalidDatesExceptionAsync()
        {
            //Arrange
            DateTime expiracao = DateTime.Now;
            DateTime inicio = DateTime.Now.AddMonths(1);
            bool status = true;

            //Act & Assert
            Assert.Throws<DomainPeriodOfInvalidDatesException>(() => new Assinatura(inicio, expiracao, It.IsAny<bool>()));
        }

        [Fact]
        public void InstanciaAssinatura_InformandoExpiracaoMaiorInicio_CriaObjetoAssinatura()
        {
            //Arrange
            DateTime expiracao = DateTime.Now.AddMonths(1);
            DateTime inicio = DateTime.Now;
            bool status = true;

            //Act
            Assinatura assinatura = new Assinatura(inicio, expiracao, It.IsAny<bool>());

            //Assert
            Assert.True(assinatura.Fim == expiracao && assinatura.Inicio == inicio);
        }

        [Fact]
        public void ExtendeAssinaturaNaoPaga_RetornaDomainRenewalNotPaidException()
        {
            //Act
            Assinatura assinatura = new Assinatura(DateTime.Now, DateTime.Now.AddMonths(1), false);
            DateTime novaDataFinal = DateTime.Now.AddMonths(2);
            bool status = false;

            //Act & Assert
            Assert.Throws<DomainRenewalNotPaidException>(() => assinatura.ValidarAtualizacao(status, novaDataFinal));
        }

        [Fact]
        public void ExtendeAssinaturaPaga_AtualizaAssinatura()
        {
            //Act
            Assinatura assinatura = new Assinatura(DateTime.Now, DateTime.Now.AddMonths(1), true);
            DateTime novaDataFinal = DateTime.Now.AddMonths(2);
            bool status = true;

            //Act
            assinatura.ValidarAtualizacao(status, novaDataFinal);

            //Assert
            Assert.True(assinatura.Fim == novaDataFinal && assinatura.Status == status);
        }
    }
}
