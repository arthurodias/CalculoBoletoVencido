using Moq;
using Boleto.Domain.Intefaces.Repositories;
using Boleto.Service.Services;
using Boleto.Domain.Entities;
using Boleto.Domain.Intefaces.Proxy;

namespace Boleto.Tests.UnitTests
{
    public class BoletoServiceTests
    {
        [Fact]
        public async Task CalcularValorBoletoAsync_ValidBarcodeAndPaymentDate_ReturnsCorrectValues()
        {
            var token = "valid_token";
            var barcode = "34191790010104351004791020150008291070026000";
            var paymentDate = new DateTime(2024, 5, 20);
            var mockBoleto = new BoletoModel
            {
                Type = "NPC",
                DueDate = new DateTime(2024, 5, 18),
                Amount = 100.0m
            };
            var mockApiClient = new Mock<IBoletoApiClient>();
            mockApiClient.Setup(client => client.GetBoletoAsync(barcode, token)).ReturnsAsync(mockBoleto);

            var mockRepository = new Mock<IBoletoRepository>();
            var service = new BoletoService(mockRepository.Object, mockApiClient.Object);

            var result = await service.CalcularValorBoletoAsync(barcode, paymentDate, token);

            Assert.Equal(mockBoleto.Amount + 0.066m * mockBoleto.Amount + 0.02m * mockBoleto.Amount, result.Amount);
            Assert.Equal(mockBoleto.Amount, result.OriginalAmount);
            Assert.Equal("2024-05-18", result.DueDate);
            Assert.Equal("2024-05-20", result.PaymentDate);
            Assert.Equal(0.066m * mockBoleto.Amount, result.InterestAmountCalculated);
            Assert.Equal(0.02m * mockBoleto.Amount, result.FineAmountCalculated);
        }

        [Fact]
        public async Task CalcularValorBoletoAsync_InvalidBoletoType_ThrowsArgumentException()
        {
            var token = "valid_token";
            var barcode = "valid_barcode";
            var paymentDate = new DateTime(2024, 5, 20);
            var mockBoleto = new BoletoModel { Type = "INVALID_TYPE" };
            var mockApiClient = new Mock<IBoletoApiClient>();
            mockApiClient.Setup(client => client.GetBoletoAsync(barcode, token)).ReturnsAsync(mockBoleto);

            var mockRepository = new Mock<IBoletoRepository>();
            var service = new BoletoService(mockRepository.Object, mockApiClient.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => service.CalcularValorBoletoAsync(barcode, paymentDate, token));
        }

        [Fact]
        public async Task CalcularValorBoletoAsync_NotLatePayment_ThrowsArgumentException()
        {
            var token = "valid_token";
            var barcode = "valid_barcode";
            var paymentDate = new DateTime(2024, 5, 18);
            var mockBoleto = new BoletoModel { Type = "NPC", DueDate = paymentDate.AddDays(1) };
            var mockApiClient = new Mock<IBoletoApiClient>();
            mockApiClient.Setup(client => client.GetBoletoAsync(barcode, token)).ReturnsAsync(mockBoleto);

            var mockRepository = new Mock<IBoletoRepository>();
            var service = new BoletoService(mockRepository.Object, mockApiClient.Object);

            await Assert.ThrowsAsync<ArgumentException>(() => service.CalcularValorBoletoAsync(barcode, paymentDate, token));
        }
    }
}