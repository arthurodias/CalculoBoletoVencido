using Boleto.Domain.Entities;
using Boleto.Domain.Intefaces.Repositories;
using Boleto.Domain.Models;
using Boleto.Service.ExternalServices;
using Boleto.Service.Services;
using Moq;

namespace Boleto.Tests
{
    public class BoletoServiceTests
    {
        [Fact]
        public async Task CalcularValorBoleto_BoletoVencido_CalculaCorretamente()
        {
            var mockRepo = new Mock<IBoletoRepository>();
            var mockApiClient = new Mock<BoletoApiClient>();
            mockApiClient.Setup(repo => repo.GetBoletoAsync(It.IsAny<string>()))
                .ReturnsAsync(new BoletoModel
                {
                    Code = "1234567890",
                    DueDate = DateTime.Now.AddDays(-10),
                    Amount = 100,
                    Type = "NPC"
                });

            var boletoService = new BoletoService(mockRepo.Object, mockApiClient.Object);
            var paymentDate = DateTime.Now;

            var valorCalculado = await boletoService.CalcularValorBoletoAsync("1234567890", paymentDate);

            var expectedJuros = 100 * 0.033m / 100 * 10; // 0.033% por dia de atraso
            var expectedMulta = 100 * 0.02m; // 2% de multa
            var expectedValorFinal = 100 + expectedJuros + expectedMulta;

            Assert.Equal(expectedValorFinal, valorCalculado.Amount);
        }

        [Fact]
        public async Task CalcularValorBoleto_BoletoNaoVencido_DisparaExcecao()
        {
            var mockRepo = new Mock<IBoletoRepository>();
            var mockApiClient = new Mock<BoletoApiClient>();

            mockApiClient.Setup(repo => repo.GetBoletoAsync(It.IsAny<string>()))
                .ReturnsAsync(new BoletoModel
                {
                    Code = "1234567890",
                    DueDate = DateTime.Now.AddDays(10),
                    Amount = 100,
                    Type = "NPC"
                });

            var boletoService = new BoletoService(mockRepo.Object, mockApiClient.Object);
            var paymentDate = DateTime.Now;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => 
                await boletoService.CalcularValorBoletoAsync("1234567890", paymentDate));
        }

        [Fact]
        public async Task CalcularValorBoleto_TipoNaoNPC_DisparaExcecao()
        {
            var mockRepo = new Mock<IBoletoRepository>();
            var mockApiClient = new Mock<BoletoApiClient>();

            mockApiClient.Setup(repo => repo.GetBoletoAsync(It.IsAny<string>()))
                .ReturnsAsync(new BoletoModel
                {
                    Code = "1234567890",
                    DueDate = DateTime.Now.AddDays(-10),
                    Amount = 100,
                    Type = "NORMAL"
                });

            var boletoService = new BoletoService(mockRepo.Object, mockApiClient.Object);
            var paymentDate = DateTime.Now;

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
                await boletoService.CalcularValorBoletoAsync("1234567890", paymentDate));
        }
    }
}