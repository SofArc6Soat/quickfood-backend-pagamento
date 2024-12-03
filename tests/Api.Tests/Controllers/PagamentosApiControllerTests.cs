using Api.Controllers;
using Controllers;
using Core.Domain.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.Tests.Controllers;

public class PagamentosApiControllerTests
{
    private readonly Mock<IPagamentoController> _pagamentoControllerMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly PagamentosApiController _controller;

    public PagamentosApiControllerTests()
    {
        _pagamentoControllerMock = new Mock<IPagamentoController>();
        _notificadorMock = new Mock<INotificador>();
        _controller = new PagamentosApiController(_pagamentoControllerMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task ObterPagamentoPorPedido_ExcecaoLancada_DeveRetornarStatusCode500()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pagamentoControllerMock.Setup(x => x.ObterPagamentoPorPedidoAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro"));

        // Act
        var result = await _controller.ObterPagamentoPorPedido(pedidoId, CancellationToken.None);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task Checkout_CheckoutBemSucedido_DeveRetornarOk()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pagamentoControllerMock.Setup(x => x.EfetuarCheckoutAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Checkout(pedidoId, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True(((dynamic)okResult.Value).Success);
    }

    [Fact]
    public async Task Checkout_ExcecaoLancada_DeveRetornarStatusCode500()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pagamentoControllerMock.Setup(x => x.EfetuarCheckoutAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro"));

        // Act
        var result = await _controller.Checkout(pedidoId, CancellationToken.None);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task Checkout_ModelStateInvalido_DeveRetornarBadRequest()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _controller.ModelState.AddModelError("Erro", "Erro de validação");

        // Act
        var result = await _controller.Checkout(pedidoId, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task Notificacoes_NotificacaoBemSucedida_DeveRetornarOk()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pagamentoControllerMock.Setup(x => x.NotificarPagamentoAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Notificacoes(pedidoId, CancellationToken.None);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True(((dynamic)okResult.Value).Success);
    }

    [Fact]
    public async Task Notificacoes_ExcecaoLancada_DeveRetornarStatusCode500()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pagamentoControllerMock.Setup(x => x.NotificarPagamentoAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro"));

        // Act
        var result = await _controller.Notificacoes(pedidoId, CancellationToken.None);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task Notificacoes_ModelStateInvalido_DeveRetornarBadRequest()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _controller.ModelState.AddModelError("Erro", "Erro de validação");

        // Act
        var result = await _controller.Notificacoes(pedidoId, CancellationToken.None);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }
}
