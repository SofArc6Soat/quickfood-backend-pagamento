using Core.Domain.Notificacoes;
using Domain.Entities;
using Gateways;
using Moq;
using UseCases;

namespace Application.Tests.UseCases;

public class PagamentoUseCaseTests
{
    private readonly Mock<IPedidoGateway> _pedidoGatewayMock;
    private readonly Mock<IPagamentoGateway> _pagamentoGatewayMock;
    private readonly Mock<INotificador> _notificadorMock;
    private readonly PagamentoUseCase _pagamentoUseCase;

    public PagamentoUseCaseTests()
    {
        _pedidoGatewayMock = new Mock<IPedidoGateway>();
        _pagamentoGatewayMock = new Mock<IPagamentoGateway>();
        _notificadorMock = new Mock<INotificador>();
        _pagamentoUseCase = new PagamentoUseCase(_pedidoGatewayMock.Object, _pagamentoGatewayMock.Object, _notificadorMock.Object);
    }

    [Fact]
    public async Task EfetuarCheckoutAsync_PedidoNaoEncontrado_DeveNotificarERetornarFalse()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pedidoGatewayMock.Setup(x => x.ObterPedidoAsync(pedidoId, It.IsAny<CancellationToken>())).ReturnsAsync((Pedido)null);

        // Act
        var result = await _pagamentoUseCase.EfetuarCheckoutAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.False(result);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }

    [Fact]
    public async Task EfetuarCheckoutAsync_PagamentoExistente_DeveNotificarERetornarFalse()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var pedido = new Pedido(pedidoId, 123, 100m, DateTime.Now);
        var pagamento = new Pagamento(pedidoId, 100m);
        _pedidoGatewayMock.Setup(x => x.ObterPedidoAsync(pedidoId, It.IsAny<CancellationToken>())).ReturnsAsync(pedido);
        _pagamentoGatewayMock.Setup(x => x.ObterPagamentoPorPedido(pedidoId, It.IsAny<CancellationToken>())).Returns(pagamento);

        // Act
        var result = await _pagamentoUseCase.EfetuarCheckoutAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.False(result);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }

    [Fact]
    public async Task EfetuarCheckoutAsync_Sucesso_DeveCadastrarPagamentoERetornarTrue()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var pedido = new Pedido(pedidoId, 123, 100m, DateTime.Now);
        _pedidoGatewayMock.Setup(x => x.ObterPedidoAsync(pedidoId, It.IsAny<CancellationToken>())).ReturnsAsync(pedido);
        _pagamentoGatewayMock.Setup(x => x.ObterPagamentoPorPedido(pedidoId, It.IsAny<CancellationToken>())).Returns((Pagamento)null);
        _pedidoGatewayMock.Setup(x => x.AtualizarPedidoAsync(pedido, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _pagamentoGatewayMock.Setup(x => x.CadastrarPagamentoAsync(It.IsAny<Pagamento>(), It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _pagamentoGatewayMock.Setup(x => x.GerarQrCodePixGatewayPagamento(It.IsAny<Pagamento>())).Returns("QRCode");

        // Act
        var result = await _pagamentoUseCase.EfetuarCheckoutAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.True(result);
        _pagamentoGatewayMock.Verify(x => x.CadastrarPagamentoAsync(It.IsAny<Pagamento>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task NotificarPagamentoAsync_PagamentoNaoEncontrado_DeveNotificarERetornarFalse()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        _pagamentoGatewayMock.Setup(x => x.ObterPagamentoPorPedido(pedidoId, It.IsAny<CancellationToken>())).Returns((Pagamento)null);

        // Act
        var result = await _pagamentoUseCase.NotificarPagamentoAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.False(result);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }

    [Fact]
    public async Task NotificarPagamentoAsync_PedidoNaoEncontrado_DeveNotificarERetornarFalse()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var pagamento = new Pagamento(pedidoId, 100m);
        _pagamentoGatewayMock.Setup(x => x.ObterPagamentoPorPedido(pedidoId, It.IsAny<CancellationToken>())).Returns(pagamento);
        _pedidoGatewayMock.Setup(x => x.ObterPedidoAsync(pedidoId, It.IsAny<CancellationToken>())).ReturnsAsync((Pedido)null);

        // Act
        var result = await _pagamentoUseCase.NotificarPagamentoAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.False(result);
        _notificadorMock.Verify(x => x.Handle(It.IsAny<Notificacao>()), Times.Once);
    }

    [Fact]
    public async Task NotificarPagamentoAsync_Sucesso_DeveAtualizarPedidoENotificarPagamentoERetornarTrue()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var pedido = new Pedido(pedidoId, 123, 100m, DateTime.Now);
        var pagamento = new Pagamento(pedidoId, 100m);
        _pagamentoGatewayMock.Setup(x => x.ObterPagamentoPorPedido(pedidoId, It.IsAny<CancellationToken>())).Returns(pagamento);
        _pedidoGatewayMock.Setup(x => x.ObterPedidoAsync(pedidoId, It.IsAny<CancellationToken>())).ReturnsAsync(pedido);
        _pedidoGatewayMock.Setup(x => x.AtualizarPedidoAsync(pedido, It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _pagamentoGatewayMock.Setup(x => x.NotificarPagamentoAsync(pagamento, It.IsAny<CancellationToken>())).ReturnsAsync(true);

        // Act
        var result = await _pagamentoUseCase.NotificarPagamentoAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.True(result);
        _pedidoGatewayMock.Verify(x => x.AtualizarPedidoAsync(pedido, It.IsAny<CancellationToken>()), Times.Once);
        _pagamentoGatewayMock.Verify(x => x.NotificarPagamentoAsync(pagamento, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterPagamentoPorPedidoAsync_DeveRetornarPagamento()
    {
        // Arrange
        var pedidoId = Guid.NewGuid();
        var pagamento = "Pagamento";
        _pagamentoGatewayMock.Setup(x => x.ObterPagamentoPorPedidoAsync(pedidoId, It.IsAny<CancellationToken>())).ReturnsAsync(pagamento);

        // Act
        var result = await _pagamentoUseCase.ObterPagamentoPorPedidoAsync(pedidoId, CancellationToken.None);

        // Assert
        Assert.Equal(pagamento, result);
    }
}
