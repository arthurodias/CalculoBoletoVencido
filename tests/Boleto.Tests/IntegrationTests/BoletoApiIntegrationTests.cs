using Boleto.Api;
using Boleto.Domain.Intefaces.Proxy;
using Boleto.Domain.Intefaces.Repositories;
using Boleto.Domain.Models;
using Boleto.Infrastructure.Data;
using Boleto.Infrastructure.Repositories;
using Boleto.Service.ExternalServices;
using Boleto.Service.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Boleto.Tests.IntegrationTests
{
    public class BoletoApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public BoletoApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("34191790010104351004791020150008291070026000", "2022-09-03", 260.00)]
        [InlineData("34191790010104351004791020150008191070069000", "2022-08-07", 690.00)]
        public async Task CalculateBoleto_IntegrationTestOK(string code, string dueDateString, decimal amount)
        {
            var dueDate = DateTime.Parse(dueDateString);
            var client = _factory.CreateClient();

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    code,
                    due_date = dueDateString,
                    amount,
                    recipient_name = "Test Recipient",
                    recipient_document = "12345678900",
                    type = "NPC"
                }), Encoding.UTF8, "application/json")
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var services = new ServiceCollection();

            services.AddDbContext<BoletoDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });

            services.AddHttpClient<BoletoApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://vagas.builders/api/builders/");
            }).ConfigurePrimaryHttpMessageHandler(() => mockHttpMessageHandler.Object);

            // Adiciona o repositório e o serviço de boleto
            services.AddTransient<IBoletoRepository, BoletoRepository>();
            services.AddTransient<IBoletoApiClient, BoletoApiClient>();
            services.AddTransient<BoletoService>();

            // Construção do serviço
            var serviceProvider = services.BuildServiceProvider();
            var dbContext = serviceProvider.GetRequiredService<BoletoDbContext>();

            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.EnsureCreatedAsync();

            var boletoService = serviceProvider.GetRequiredService<BoletoService>();

            var requestBody = new
            {
                bar_code = code,
                payment_date = DateTime.Now.ToString("yyyy-MM-dd")
            };

            var responseToken = await client.GetAsync("/test");
            var token = JsonConvert.DeserializeObject<TokenResponse>(await responseToken.Content.ReadAsStringAsync());
            Assert.NotNull(token);

            var response = await boletoService.CalcularValorBoletoAsync(code, DateTime.Now, token.Token);

            int daysLate = (DateTime.Now - dueDate).Days;
            decimal expectedInterest = daysLate * 0.033m * amount;
            decimal expectedFine = 0.02m * amount;
            decimal expectedAmount = amount + expectedInterest + expectedFine;

            Assert.NotNull(response);
            Assert.Equal(expectedAmount, response.Amount);
            Assert.Equal(expectedInterest, response.InterestAmountCalculated);
            Assert.Equal(expectedFine, response.FineAmountCalculated);
            Assert.Equal(dueDate.ToString("yyyy-MM-dd"), response.DueDate);

            await dbContext.Database.EnsureDeletedAsync();
        }

        [Theory]
        [InlineData("34199800020104352008771020110004191070010000", "2024-05-21", 100.00)]
        public async Task CalculateBoleto_IntegrationTestBadRequestBoletoVencido(string code, string dueDateString, decimal amount)
        {
            var dueDate = DateTime.Parse(dueDateString);
            var client = _factory.CreateClient();

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    code,
                    due_date = dueDateString,
                    amount,
                    recipient_name = "Test Recipient",
                    recipient_document = "12345678900",
                    type = "NPC"
                }), Encoding.UTF8, "application/json")
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var services = new ServiceCollection();
            services.AddHttpClient<BoletoApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://vagas.builders/api/builders/");
            }).ConfigurePrimaryHttpMessageHandler(() => mockHttpMessageHandler.Object);

            var mockRepository = new Mock<IBoletoRepository>();

            var serviceProvider = services.BuildServiceProvider();
            var boletoApiClient = serviceProvider.GetRequiredService<BoletoApiClient>();
            var boletoService = new BoletoService(mockRepository.Object, boletoApiClient);

            var requestBody = new
            {
                bar_code = code,
                payment_date = DateTime.Now.ToString("yyyy-MM-dd")
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/boleto", requestContent);

            var responseString = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("O boleto não está vencido.", responseString);
        }

        [Theory]
        [InlineData("34197650070104357008271020110004991070040000", "2024-03-01", 400.00)]
        public async Task CalculateBoleto_IntegrationTestBadRequestBoletoNormal(string code, string dueDateString, decimal amount)
        {
            var dueDate = DateTime.Parse(dueDateString);
            var client = _factory.CreateClient();

            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    code,
                    due_date = dueDateString,
                    amount,
                    recipient_name = "Test Recipient",
                    recipient_document = "12345678900",
                    type = "Normal"
                }), Encoding.UTF8, "application/json")
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var services = new ServiceCollection();
            services.AddHttpClient<BoletoApiClient>(client =>
            {
                client.BaseAddress = new Uri("https://vagas.builders/api/builders/");
            }).ConfigurePrimaryHttpMessageHandler(() => mockHttpMessageHandler.Object);

            var mockRepository = new Mock<IBoletoRepository>();

            var serviceProvider = services.BuildServiceProvider();
            var boletoApiClient = serviceProvider.GetRequiredService<BoletoApiClient>();
            var boletoService = new BoletoService(mockRepository.Object, boletoApiClient);

            var requestBody = new
            {
                bar_code = code,
                payment_date = DateTime.Now.ToString("yyyy-MM-dd")
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("/boleto", requestContent);

            var responseString = await response.Content.ReadAsStringAsync();

            var responseObject = JsonConvert.DeserializeObject<dynamic>(responseString);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Contains("Apenas boletos do tipo NPC podem ser calculados.", responseString);
        }
    }
}