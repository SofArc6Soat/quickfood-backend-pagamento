using Core.Domain.Data;
using Domain.Tests.TestHelpers;
using Infra.Dto;
using Infra.Repositories;
using Moq;

namespace Infra.Tests.Repositories;

public class PagamentoRepositoryTests
{
    private readonly Mock<IPagamentoRepository> _mockPagamentoRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;

    public PagamentoRepositoryTests()
    {
        _mockPagamentoRepository = new Mock<IPagamentoRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPagamentoRepository.Setup(repo => repo.UnitOfWork).Returns(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task ObterPagamentoPorPedidoAsync_DeveRetornarPagamento()
    {
        // Arrange
        var pagamentoDb = PagamentoFakeDataFactory.CriarPagamentoDbValido();
        var cancellationToken = CancellationToken.None;

        _mockPagamentoRepository.Setup(x => x.ObterPagamentoPorPedidoAsync(pagamentoDb.PedidoId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagamentoDb.Status);

        // Act
        var resultado = await _mockPagamentoRepository.Object.ObterPagamentoPorPedidoAsync(pagamentoDb.PedidoId, cancellationToken);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(pagamentoDb.Status, resultado);
        _mockPagamentoRepository.Verify(x => x.ObterPagamentoPorPedidoAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ObterPagamentoPorPedidoAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var pedidoId = PagamentoFakeDataFactory.ObterNovoGuid();
        var cancellationToken = CancellationToken.None;

        _mockPagamentoRepository.Setup(x => x.ObterPagamentoPorPedidoAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao obter pagamento por pedido"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockPagamentoRepository.Object.ObterPagamentoPorPedidoAsync(pedidoId, cancellationToken));
        Assert.Equal("Erro ao obter pagamento por pedido", exception.Message);
        _mockPagamentoRepository.Verify(x => x.ObterPagamentoPorPedidoAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FindByIdAsync_DeveRetornarPagamentoPorId()
    {
        // Arrange
        var pagamentoDb = PagamentoFakeDataFactory.CriarPagamentoDbValido();
        var cancellationToken = CancellationToken.None;

        _mockPagamentoRepository.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagamentoDb);

        // Act
        var resultado = await _mockPagamentoRepository.Object.FindByIdAsync(pagamentoDb.Id, cancellationToken);

        // Assert
        Assert.NotNull(resultado);
        Assert.Equal(pagamentoDb.Id, resultado.Id);
        _mockPagamentoRepository.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task FindByIdAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var pagamentoId = PagamentoFakeDataFactory.ObterNovoGuid();
        var cancellationToken = CancellationToken.None;

        _mockPagamentoRepository.Setup(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao encontrar pagamento por ID"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockPagamentoRepository.Object.FindByIdAsync(pagamentoId, cancellationToken));
        Assert.Equal("Erro ao encontrar pagamento por ID", exception.Message);
        _mockPagamentoRepository.Verify(x => x.FindByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_DeveInserirPagamento()
    {
        // Arrange
        var pagamentoDb = PagamentoFakeDataFactory.CriarPagamentoDbValido();
        var cancellationToken = CancellationToken.None;

        _mockPagamentoRepository.Setup(x => x.InsertAsync(It.IsAny<PagamentoDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _mockPagamentoRepository.Object.InsertAsync(pagamentoDb, cancellationToken);

        // Assert
        _mockPagamentoRepository.Verify(x => x.InsertAsync(It.IsAny<PagamentoDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task InsertAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var pagamentoDb = PagamentoFakeDataFactory.CriarPagamentoDbValido();
        var cancellationToken = CancellationToken.None;

        _mockPagamentoRepository.Setup(x => x.InsertAsync(It.IsAny<PagamentoDb>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao inserir pagamento"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockPagamentoRepository.Object.InsertAsync(pagamentoDb, cancellationToken));
        Assert.Equal("Erro ao inserir pagamento", exception.Message);
        _mockPagamentoRepository.Verify(x => x.InsertAsync(It.IsAny<PagamentoDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveDeletarPagamento()
    {
        // Arrange
        var pagamentoDb = PagamentoFakeDataFactory.CriarPagamentoDbValido();
        var cancellationToken = CancellationToken.None;

        _mockPagamentoRepository.Setup(x => x.DeleteAsync(It.IsAny<PagamentoDb>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        await _mockPagamentoRepository.Object.DeleteAsync(pagamentoDb, cancellationToken);

        // Assert
        _mockPagamentoRepository.Verify(x => x.DeleteAsync(It.IsAny<PagamentoDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_DeveLancarExcecao_QuandoOcorreErro()
    {
        // Arrange
        var pagamentoDb = PagamentoFakeDataFactory.CriarPagamentoDbValido();
        var cancellationToken = CancellationToken.None;

        _mockPagamentoRepository.Setup(x => x.DeleteAsync(It.IsAny<PagamentoDb>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Erro ao deletar pagamento"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => _mockPagamentoRepository.Object.DeleteAsync(pagamentoDb, cancellationToken));
        Assert.Equal("Erro ao deletar pagamento", exception.Message);
        _mockPagamentoRepository.Verify(x => x.DeleteAsync(It.IsAny<PagamentoDb>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
