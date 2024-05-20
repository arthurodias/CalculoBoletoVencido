using Boleto.Api;
using Boleto.Api.Models;
using Boleto.Domain.Entities;
using Boleto.Domain.Intefaces.Repositories;
using Boleto.Domain.Models;
using Boleto.Service.ExternalServices;
using Boleto.Service.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Text;

public class BoletoApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public BoletoApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("34191790010104351004791020150008291070026000", "2024-05-01", 100.00)]
    [InlineData("34191790010104351004791020150008191070069000", "2024-04-15", 200.00)]
    [InlineData("34199800020104352008771020110004191070010000", "2024-04-01", 300.00)]
    [InlineData("34197650070104357008271020110004991070040000", "2024-03-01", 400.00)]
    public async Task CalculateBoleto_IntegrationTest(string code, string dueDateString, decimal amount)
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

        // Set up the HttpClient in the service provider
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
        response.EnsureSuccessStatusCode();

        var responseString = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<BoletoResponse>(responseString);

       // mockRepository.Verify(r => r.Save(It.Is<BoletoModel>(b => b.Code == code && b.DueDate == dueDate && b.Amount == amount)), Times.Once);

        int daysLate = (DateTime.Now - dueDate).Days;
        decimal expectedInterest = daysLate * 0.033m * amount;
        decimal expectedFine = 0.02m * amount;
        decimal expectedAmount = amount + expectedInterest + expectedFine;

        Assert.NotNull(responseObject);
        Assert.Equal(expectedAmount, responseObject.Amount);
        Assert.Equal(expectedInterest, responseObject.InterestAmountCalculated);
        Assert.Equal(expectedFine, responseObject.FineAmountCalculated);
        Assert.Equal(dueDate.ToString("yyyy-MM-dd"), responseObject.DueDate);
    }
}