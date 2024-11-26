using Moq;
using System.Net;
using System.Net.Http.Json;

namespace SmokeTests.SmokeTests;

public class PagamentosApiControllerSmokeTest
{
    private readonly HttpClient _client;
    private readonly Mock<HttpMessageHandler> _handlerMock;

    public PagamentosApiControllerSmokeTest()
    {
        _handlerMock = MockHttpMessageHandler.SetupMessageHandlerMock(HttpStatusCode.OK, "{\"Success\":true}");
        _client = new HttpClient(_handlerMock.Object)
        {
            BaseAddress = new Uri("http://localhost")
        };
    }

    [Fact]
    public async Task Get_ObterPagamentoPorPedidoEndpoint_ReturnsSuccess()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/pagamentos/{pedidoId}");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.NotNull(response.Content.Headers.ContentType);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Get_ObterPagamentoPorPedidoEndpoint_ReturnsNotFound()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _handlerMock.SetupRequest(HttpMethod.Get, $"http://localhost/pagamentos/{pedidoId}", HttpStatusCode.NotFound, "{\"Success\":false, \"Errors\":[\"Pagamento não encontrado\"]}");

        // Act
        var response = await _client.GetAsync($"/pagamentos/{pedidoId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Pagamento não encontrado", content);
    }

    [Fact]
    public async Task Post_CheckoutEndpoint_ReturnsSuccess()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();

        // Act
        var response = await _client.PostAsJsonAsync($"/pagamentos/checkout/{pedidoId}", new { });

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Status Code: {response.StatusCode}, Content: {content}");
        }

        response.EnsureSuccessStatusCode();
        Assert.NotNull(response.Content.Headers.ContentType);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Post_CheckoutEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _handlerMock.SetupRequest(HttpMethod.Post, $"http://localhost/pagamentos/checkout/{pedidoId}", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao efetuar checkout\"]}");

        // Act
        var response = await _client.PostAsJsonAsync($"/pagamentos/checkout/{pedidoId}", new { });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao efetuar checkout", content);
    }

    [Fact]
    public async Task Post_NotificacoesEndpoint_ReturnsSuccess()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();

        // Act
        var response = await _client.PostAsJsonAsync($"/pagamentos/notificacoes/{pedidoId}", new { });

        // Assert
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Status Code: {response.StatusCode}, Content: {content}");
        }

        response.EnsureSuccessStatusCode();
        Assert.NotNull(response.Content.Headers.ContentType);
        Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
    }

    [Fact]
    public async Task Post_NotificacoesEndpoint_ReturnsBadRequest()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _handlerMock.SetupRequest(HttpMethod.Post, $"http://localhost/pagamentos/notificacoes/{pedidoId}", HttpStatusCode.BadRequest, "{\"Success\":false, \"Errors\":[\"Erro ao notificar pagamento\"]}");

        // Act
        var response = await _client.PostAsJsonAsync($"/pagamentos/notificacoes/{pedidoId}", new { });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Erro ao notificar pagamento", content);
    }
}
