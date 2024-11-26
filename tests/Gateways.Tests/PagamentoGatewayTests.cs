using Core.Infra.MessageBroker;
using Domain.Tests.TestHelpers;
using Gateways.Dtos.Events;
using Infra.Dto;
using Infra.Repositories;
using Moq;
using System.Linq.Expressions;

namespace Gateways.Tests;

public class PagamentoGatewayTests
{
    private readonly Mock<IPagamentoRepository> _pagamentoRepositoryMock;
    private readonly Mock<ISqsService<PedidoPagoEvent>> _sqsPedidoPagoMock;
    private readonly Mock<ISqsService<PedidoPendentePagamentoEvent>> _sqsPedidoPendentePagamentoMock;
    private readonly PagamentoGateway _pagamentoGateway;

    public PagamentoGatewayTests()
    {
        _pagamentoRepositoryMock = new Mock<IPagamentoRepository>();
        _sqsPedidoPagoMock = new Mock<ISqsService<PedidoPagoEvent>>();
        _sqsPedidoPendentePagamentoMock = new Mock<ISqsService<PedidoPendentePagamentoEvent>>();
        _pagamentoGateway = new PagamentoGateway(_pagamentoRepositoryMock.Object, _sqsPedidoPagoMock.Object, _sqsPedidoPendentePagamentoMock.Object);
    }

    [Fact]
    public async Task CadastrarPagamentoAsync_DeveRetornarTrue_QuandoCadastroForBemSucedido()
    {
        // Arrange
        var pagamento = PagamentoFakeDataFactory.CriarPagamentoValido();
        var pagamentoDb = PagamentoFakeDataFactory.CriarPagamentoDbValido();

        _pagamentoRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<PagamentoDb>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _pagamentoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _sqsPedidoPendentePagamentoMock.Setup(x => x.SendMessageAsync(It.IsAny<PedidoPendentePagamentoEvent>())).ReturnsAsync(true);

        // Act
        var result = await _pagamentoGateway.CadastrarPagamentoAsync(pagamento, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CadastrarPagamentoAsync_DeveRetornarFalse_QuandoCommitFalhar()
    {
        // Arrange
        var pagamento = PagamentoFakeDataFactory.CriarPagamentoValido();

        _pagamentoRepositoryMock.Setup(x => x.InsertAsync(It.IsAny<PagamentoDb>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _pagamentoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _pagamentoGateway.CadastrarPagamentoAsync(pagamento, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task NotificarPagamentoAsync_DeveRetornarTrue_QuandoNotificacaoForBemSucedida()
    {
        // Arrange
        var pagamento = PagamentoFakeDataFactory.CriarPagamentoValido();

        _pagamentoRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<PagamentoDb>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _pagamentoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(true);
        _sqsPedidoPagoMock.Setup(x => x.SendMessageAsync(It.IsAny<PedidoPagoEvent>())).ReturnsAsync(true);

        // Act
        var result = await _pagamentoGateway.NotificarPagamentoAsync(pagamento, CancellationToken.None);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task NotificarPagamentoAsync_DeveRetornarFalse_QuandoCommitFalhar()
    {
        // Arrange
        var pagamento = PagamentoFakeDataFactory.CriarPagamentoValido();

        _pagamentoRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<PagamentoDb>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        _pagamentoRepositoryMock.Setup(x => x.UnitOfWork.CommitAsync(It.IsAny<CancellationToken>())).ReturnsAsync(false);

        // Act
        var result = await _pagamentoGateway.NotificarPagamentoAsync(pagamento, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GerarQrCodePixGatewayPagamento_DeveRetornarQrCode()
    {
        // Arrange
        var pagamento = PagamentoFakeDataFactory.CriarPagamentoValido();

        // Act
        var qrCode = _pagamentoGateway.GerarQrCodePixGatewayPagamento(pagamento);

        // Assert
        Assert.NotNull(qrCode);
        Assert.Equal(100, qrCode.Length);
    }

    [Fact]
    public void ObterPagamentoPorPedido_DeveRetornarPagamento_QuandoPagamentoExistir()
    {
        // Arrange
        var pagamentoDb = PagamentoFakeDataFactory.CriarPagamentoDbValido();

        _pagamentoRepositoryMock.Setup(x => x.Find(It.IsAny<Expression<Func<PagamentoDb, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(new List<PagamentoDb> { pagamentoDb }.AsQueryable());

        // Act
        var result = _pagamentoGateway.ObterPagamentoPorPedido(pagamentoDb.PedidoId, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(pagamentoDb.PedidoId, result.PedidoId);
    }

    [Fact]
    public void ObterPagamentoPorPedido_DeveRetornarNull_QuandoPagamentoNaoExistir()
    {
        // Arrange
        var pedidoId = PagamentoFakeDataFactory.ObterNovoGuid();

        _pagamentoRepositoryMock.Setup(x => x.Find(It.IsAny<Expression<Func<PagamentoDb, bool>>>(), It.IsAny<CancellationToken>()))
            .Returns(Enumerable.Empty<PagamentoDb>().AsQueryable());

        // Act
        var result = _pagamentoGateway.ObterPagamentoPorPedido(pedidoId, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}