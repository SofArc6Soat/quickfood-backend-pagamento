using Moq;
using UseCases;

namespace Controllers.Tests.Controllers;

public class PagamentoControllerTests
{
    private readonly Mock<IPagamentoUseCase> _pagamentoUseCaseMock;
    private readonly IPagamentoController _controller;

    public PagamentoControllerTests()
    {
        _pagamentoUseCaseMock = new Mock<IPagamentoUseCase>();
        _controller = new PagamentoController(_pagamentoUseCaseMock.Object);
    }

    [Fact]
    public async Task EfetuarCheckoutAsync_CheckoutBemSucedido_DeveRetornarTrue()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pagamentoUseCaseMock.Setup(x => x.EfetuarCheckoutAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.EfetuarCheckoutAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task EfetuarCheckoutAsync_CheckoutFalhou_DeveRetornarFalse()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pagamentoUseCaseMock.Setup(x => x.EfetuarCheckoutAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.EfetuarCheckoutAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task EfetuarCheckoutAsync_ExcecaoLancada_DeveLancarExcecao()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pagamentoUseCaseMock.Setup(x => x.EfetuarCheckoutAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _controller.EfetuarCheckoutAsync(pedidoId, CancellationToken.None));
    }

    [Fact]
    public async Task NotificarPagamentoAsync_NotificacaoBemSucedida_DeveRetornarTrue()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pagamentoUseCaseMock.Setup(x => x.NotificarPagamentoAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.NotificarPagamentoAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task NotificarPagamentoAsync_NotificacaoFalhou_DeveRetornarFalse()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pagamentoUseCaseMock.Setup(x => x.NotificarPagamentoAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.NotificarPagamentoAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task NotificarPagamentoAsync_ExcecaoLancada_DeveLancarExcecao()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pagamentoUseCaseMock.Setup(x => x.NotificarPagamentoAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _controller.NotificarPagamentoAsync(pedidoId, CancellationToken.None));
    }

    [Fact]
    public async Task ObterPagamentoPorPedidoAsync_PagamentoEncontrado_DeveRetornarPagamento()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var pagamento = "Pagamento";
        _pagamentoUseCaseMock.Setup(x => x.ObterPagamentoPorPedidoAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagamento);

        // Act
        var result = await _controller.ObterPagamentoPorPedidoAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.Equal(pagamento, result);
    }

    [Fact]
    public async Task ObterPagamentoPorPedidoAsync_PagamentoNaoEncontrado_DeveRetornarNull()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pagamentoUseCaseMock.Setup(x => x.ObterPagamentoPorPedidoAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((string)null);

        // Act
        var result = await _controller.ObterPagamentoPorPedidoAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task ObterPagamentoPorPedidoAsync_ExcecaoLancada_DeveLancarExcecao()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pagamentoUseCaseMock.Setup(x => x.ObterPagamentoPorPedidoAsync(pedidoId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _controller.ObterPagamentoPorPedidoAsync(pedidoId, CancellationToken.None));
    }
}